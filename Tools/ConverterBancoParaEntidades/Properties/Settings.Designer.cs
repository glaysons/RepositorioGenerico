﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConverterBancoParaEntidades.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server=(local); Database=RepositorioGenerico; integrated security=false; user id=" +
            "sa; pwd=123456; min pool size=10; MultipleActiveResultSets=True;")]
        public string Conexao {
            get {
                return ((string)(this["Conexao"]));
            }
            set {
                this["Conexao"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("select name\r\nfrom sysobjects\r\nwhere xtype = \'U\'")]
        public string ScriptConsultaTabelas {
            get {
                return ((string)(this["ScriptConsultaTabelas"]));
            }
            set {
                this["ScriptConsultaTabelas"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RepositorioGenerico.Entities\r\nRepositorioGenerico.Entities.Anotacoes\r\nSystem\r\nSys" +
            "tem.Collections.Generic")]
        public string Usings {
            get {
                return ((string)(this["Usings"]));
            }
            set {
                this["Usings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Entidade")]
        public string Namespace {
            get {
                return ((string)(this["Namespace"]));
            }
            set {
                this["Namespace"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PastaDestino {
            get {
                return ((string)(this["PastaDestino"]));
            }
            set {
                this["PastaDestino"] = value;
            }
        }
    }
}
