﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5485
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace KeyboardManiac.Core.Config {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/Settings.xsd", IsNullable=false)]
    public partial class KeyboardManiacSettings {
        
        private KeyboardManiacSettingsGlobalSettings globalSettingsField;
        
        private KeyboardManiacSettingsEngineSettings engineSettingsField;
        
        private KeyboardManiacSettingsGuiSettings guiSettingsField;
        
        private KeyboardManiacSettingsHotkey[] hotkeysField;
        
        private KeyboardManiacSettingsPluginType[] pluginTypesField;
        
        private KeyboardManiacSettingsPlugin[] pluginsField;
        
        /// <remarks/>
        public KeyboardManiacSettingsGlobalSettings GlobalSettings {
            get {
                return this.globalSettingsField;
            }
            set {
                this.globalSettingsField = value;
            }
        }
        
        /// <remarks/>
        public KeyboardManiacSettingsEngineSettings EngineSettings {
            get {
                return this.engineSettingsField;
            }
            set {
                this.engineSettingsField = value;
            }
        }
        
        /// <remarks/>
        public KeyboardManiacSettingsGuiSettings GuiSettings {
            get {
                return this.guiSettingsField;
            }
            set {
                this.guiSettingsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Hotkey", IsNullable=false)]
        public KeyboardManiacSettingsHotkey[] Hotkeys {
            get {
                return this.hotkeysField;
            }
            set {
                this.hotkeysField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PluginType", IsNullable=false)]
        public KeyboardManiacSettingsPluginType[] PluginTypes {
            get {
                return this.pluginTypesField;
            }
            set {
                this.pluginTypesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Plugin", IsNullable=false)]
        public KeyboardManiacSettingsPlugin[] Plugins {
            get {
                return this.pluginsField;
            }
            set {
                this.pluginsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsGlobalSettings : SettingCollection {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Settings.xsd")]
    public partial class SettingCollection {
        
        private SettingCollectionSetting[] settingField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Setting")]
        public SettingCollectionSetting[] Setting {
            get {
                return this.settingField;
            }
            set {
                this.settingField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class SettingCollectionSetting : Setting {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Settings.xsd")]
    public partial class Setting {
        
        private string keyField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsEngineSettings : SettingCollection {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsGuiSettings : SettingCollection {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsHotkey {
        
        private string keyField;
        
        private HotKeyModifier modifierField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public HotKeyModifier modifier {
            get {
                return this.modifierField;
            }
            set {
                this.modifierField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Settings.xsd")]
    public enum HotKeyModifier {
        
        /// <remarks/>
        ALT,
        
        /// <remarks/>
        CTRL,
        
        /// <remarks/>
        NONE,
        
        /// <remarks/>
        SHIFT,
        
        /// <remarks/>
        WIN,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsPluginType {
        
        private string idField;
        
        private string classNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string className {
            get {
                return this.classNameField;
            }
            set {
                this.classNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsPlugin : SettingCollection {
        
        private KeyboardManiacSettingsPluginAlias[] aliasField;
        
        private string typeIdField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Alias")]
        public KeyboardManiacSettingsPluginAlias[] Alias {
            get {
                return this.aliasField;
            }
            set {
                this.aliasField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeId {
            get {
                return this.typeIdField;
            }
            set {
                this.typeIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/Settings.xsd")]
    public partial class KeyboardManiacSettingsPluginAlias {
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
}
