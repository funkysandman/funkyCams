using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PylonC.NETSupportLibrary;
using PylonC.NET;

namespace PylonC.NETSupportLibrary
{
    /* Displays a combo box with the data of a GenICam enumeration and the name of the node. */
    public partial class EnumerationComboBoxUserControl : UserControl
    {
        private string name = "ValueName"; /* The name of the node. */
        private ImageProvider m_imageProvider = null; /* The image provider providing the node handle and status events. */
        private NODE_HANDLE m_hNode = new NODE_HANDLE(); /* The handle of the node. */
        private NODE_CALLBACK_HANDLE m_hCallbackHandle = new NODE_CALLBACK_HANDLE(); /* The handle of the node callback. */
        private NodeCallbackHandler m_nodeCallbackHandler = new NodeCallbackHandler(); /* The callback handler. */


        /* Used for connecting an image provider providing the node handle and status events. */
        public ImageProvider MyImageProvider
        {
            set
            {
                m_imageProvider = value;
                /* Image provider has been connected. */
                if (m_imageProvider != null)
                {
                    /* Register for the events of the image provider needed for proper operation. */
                    m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(DeviceOpenedEventHandler);
                    m_imageProvider.DeviceClosingEvent += new ImageProvider.DeviceClosingEventHandler(DeviceClosingEventHandler);
                    /* Update the control values. */
                    UpdateValues();
                }
                else /* Image provider has been disconnected. */
                {
                    Reset();
                }
            }
        }

        [Description("The GenICam node name representing an enumeration, e.g. TestImageSelector. The pylon Viewer tool feature tree can be used to get the name and the type of a node.")]
        public string NodeName
        {
            get { return name; }
            set { name = value; labelName.Text = name + ":"; }
        }

        /* A device has been opened. Update the control. */
        private void DeviceOpenedEventHandler()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceOpenedEventHandler(DeviceOpenedEventHandler));
                return;
            }
            try
            {
                /* Get the node. */
                m_hNode = m_imageProvider.GetNodeFromDevice(name);
                /* Register for changes. */
                m_hCallbackHandle = GenApi.NodeRegisterCallback(m_hNode, m_nodeCallbackHandler);
                /* Update the displayed name. */
                labelName.Text = GenApi.NodeGetDisplayName(m_hNode) + ":";
                /* Update the control values. */
                UpdateValues();
            }
            catch
            {
                /* If errors occurred disable the control. */
                Reset();
            }
        }

        /* The device has been closed. Update the control. */
        private void DeviceClosingEventHandler()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceRemovedEventHandler(DeviceClosingEventHandler));
                return;
            }
            Reset();
        }

        /* The node state has changed. Update the control. */
        private void NodeCallbackEventHandler(NODE_HANDLE handle)
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new NodeCallbackHandler.NodeCallback(NodeCallbackEventHandler), handle);
                return;
            }
            if (handle.Equals(m_hNode))
            {
                /* Update the control values. */
                UpdateValues();
            }
        }

        /* Deactivate the control and deregister the callback. */
        private void Reset()
        {
            if (m_hNode.IsValid && m_hCallbackHandle.IsValid)
            {
                try
                {
                    GenApi.NodeDeregisterCallback(m_hNode, m_hCallbackHandle);
                }
                catch
                {
                    /* Ignore. The control is about to be disabled. */
                }
            }
            m_hNode.SetInvalid();
            m_hCallbackHandle.SetInvalid();

            comboBox.Enabled = false;
            labelName.Enabled = false;
        }

        /* Get the current values from the node and display them. */
        private void UpdateValues()
        {
            try
            {
                if (m_hNode.IsValid)
                {
                    if (GenApi.NodeGetType(m_hNode) == EGenApiNodeType.EnumerationNode) /* Check is proper node type. */
                    {
                        /* Check is writable. */
                        bool writable = GenApi.NodeIsWritable(m_hNode);

                        /* Get the number of enumeration values. */
                        uint itemCount = GenApi.EnumerationGetNumEntries(m_hNode);

                        /* Clear the combo box. */
                        comboBox.Items.Clear();

                        /* Get all enumeration values, add them to the combo box, and set the selected item. */
                        string selected = GenApi.NodeToString(m_hNode);
                        for (uint i = 0; i < itemCount; i++)
                        {
                            NODE_HANDLE hEntry = GenApi.EnumerationGetEntryByIndex(m_hNode, i);
                            if (GenApi.NodeIsAvailable(hEntry))
                            {
                                comboBox.Items.Add(GenApi.NodeGetDisplayName(hEntry));
                                if (selected == GenApi.EnumerationEntryGetSymbolic(hEntry))
                                {
                                    comboBox.SelectedIndex = comboBox.Items.Count - 1;
                                }
                            }
                        }

                        /* Update accessibility. */
                        comboBox.Enabled = writable;
                        labelName.Enabled = writable;
                        return;
                    }
                }
            }
            catch
            {
                /* If errors occurred disable the control. */
            }
            Reset();
        }

        /* Set up the initial state. */
        public EnumerationComboBoxUserControl()
        {
            InitializeComponent();
            m_nodeCallbackHandler.CallbackEvent += new NodeCallbackHandler.NodeCallback(NodeCallbackEventHandler);
            Reset();
        }

        /* Handle selection changes. */
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_hNode.IsValid)
            {
                try
                {
                    /* If writable and combo box selection ok. */
                    if (GenApi.NodeIsAvailable(m_hNode) && comboBox.SelectedIndex >= 0)
                    {
                        /* Get the displayed selected enumeration value. */
                        string selectedDisplayName = comboBox.GetItemText(comboBox.Items[comboBox.SelectedIndex]);

                        /* Get the number of enumeration values. */
                        uint itemCount = GenApi.EnumerationGetNumEntries(m_hNode);

                        /* Determine the symbolic name of the selected item and set it if different. */
                        for (uint i = 0; i < itemCount; i++)
                        {
                            NODE_HANDLE hEntry = GenApi.EnumerationGetEntryByIndex(m_hNode, i);
                            if (GenApi.NodeIsAvailable(hEntry))
                            {
                                if (GenApi.NodeGetDisplayName(hEntry) == selectedDisplayName)
                                {
                                    /* Get the value to set. */
                                    string value = GenApi.EnumerationEntryGetSymbolic(hEntry);
                                    /* Set the value if other than the current value of the node. */
                                    if (GenApi.NodeToString(m_hNode) != value)
                                    {
                                        GenApi.NodeFromString(m_hNode, value);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    /* Ignore any errors here. */
                }
            }
        }
    }
}
