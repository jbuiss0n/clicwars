﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClicWars.Api.ServerAdminService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AccountResult", Namespace="http://schemas.datacontract.org/2004/07/WebServer.Services.Accounting")]
    [System.SerializableAttribute()]
    public partial class AccountResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CharacterResult", Namespace="http://schemas.datacontract.org/2004/07/WebServer.Services.Accounting")]
    [System.SerializableAttribute()]
    public partial class CharacterResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int BodyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreationDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DeathsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int KillsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SerialField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Body {
            get {
                return this.BodyField;
            }
            set {
                if ((this.BodyField.Equals(value) != true)) {
                    this.BodyField = value;
                    this.RaisePropertyChanged("Body");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreationDate {
            get {
                return this.CreationDateField;
            }
            set {
                if ((this.CreationDateField.Equals(value) != true)) {
                    this.CreationDateField = value;
                    this.RaisePropertyChanged("CreationDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Deaths {
            get {
                return this.DeathsField;
            }
            set {
                if ((this.DeathsField.Equals(value) != true)) {
                    this.DeathsField = value;
                    this.RaisePropertyChanged("Deaths");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Kills {
            get {
                return this.KillsField;
            }
            set {
                if ((this.KillsField.Equals(value) != true)) {
                    this.KillsField = value;
                    this.RaisePropertyChanged("Kills");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Serial {
            get {
                return this.SerialField;
            }
            set {
                if ((this.SerialField.Equals(value) != true)) {
                    this.SerialField = value;
                    this.RaisePropertyChanged("Serial");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServerAdminService.IServerAdminService")]
    public interface IServerAdminService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/ValidateAccount", ReplyAction="http://tempuri.org/IServerAdminService/ValidateAccountResponse")]
        ClicWars.Api.ServerAdminService.ValidateAccountResponse ValidateAccount(ClicWars.Api.ServerAdminService.ValidateAccountRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/ValidateAccount", ReplyAction="http://tempuri.org/IServerAdminService/ValidateAccountResponse")]
        System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.ValidateAccountResponse> ValidateAccountAsync(ClicWars.Api.ServerAdminService.ValidateAccountRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateAccount", ReplyAction="http://tempuri.org/IServerAdminService/CreateAccountResponse")]
        ClicWars.Api.ServerAdminService.AccountResult CreateAccount(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateAccount", ReplyAction="http://tempuri.org/IServerAdminService/CreateAccountResponse")]
        System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.AccountResult> CreateAccountAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/GetAccount", ReplyAction="http://tempuri.org/IServerAdminService/GetAccountResponse")]
        ClicWars.Api.ServerAdminService.AccountResult GetAccount(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/GetAccount", ReplyAction="http://tempuri.org/IServerAdminService/GetAccountResponse")]
        System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.AccountResult> GetAccountAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateCharacter", ReplyAction="http://tempuri.org/IServerAdminService/CreateCharacterResponse")]
        ClicWars.Api.ServerAdminService.CharacterResult CreateCharacter(string username, string name, int body);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateCharacter", ReplyAction="http://tempuri.org/IServerAdminService/CreateCharacterResponse")]
        System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.CharacterResult> CreateCharacterAsync(string username, string name, int body);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/GetCharacters", ReplyAction="http://tempuri.org/IServerAdminService/GetCharactersResponse")]
        ClicWars.Api.ServerAdminService.CharacterResult[] GetCharacters(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/GetCharacters", ReplyAction="http://tempuri.org/IServerAdminService/GetCharactersResponse")]
        System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.CharacterResult[]> GetCharactersAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateGameToken", ReplyAction="http://tempuri.org/IServerAdminService/CreateGameTokenResponse")]
        string CreateGameToken(string username, int serial);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerAdminService/CreateGameToken", ReplyAction="http://tempuri.org/IServerAdminService/CreateGameTokenResponse")]
        System.Threading.Tasks.Task<string> CreateGameTokenAsync(string username, int serial);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ValidateAccount", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ValidateAccountRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string username;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string password;
        
        public ValidateAccountRequest() {
        }
        
        public ValidateAccountRequest(string username, string password) {
            this.username = username;
            this.password = password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ValidateAccountResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ValidateAccountResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool ValidateAccountResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public ClicWars.Api.ServerAdminService.AccountResult account;
        
        public ValidateAccountResponse() {
        }
        
        public ValidateAccountResponse(bool ValidateAccountResult, ClicWars.Api.ServerAdminService.AccountResult account) {
            this.ValidateAccountResult = ValidateAccountResult;
            this.account = account;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerAdminServiceChannel : ClicWars.Api.ServerAdminService.IServerAdminService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerAdminServiceClient : System.ServiceModel.ClientBase<ClicWars.Api.ServerAdminService.IServerAdminService>, ClicWars.Api.ServerAdminService.IServerAdminService {
        
        public ServerAdminServiceClient() {
        }
        
        public ServerAdminServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServerAdminServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerAdminServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerAdminServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ClicWars.Api.ServerAdminService.ValidateAccountResponse ClicWars.Api.ServerAdminService.IServerAdminService.ValidateAccount(ClicWars.Api.ServerAdminService.ValidateAccountRequest request) {
            return base.Channel.ValidateAccount(request);
        }
        
        public bool ValidateAccount(string username, string password, out ClicWars.Api.ServerAdminService.AccountResult account) {
            ClicWars.Api.ServerAdminService.ValidateAccountRequest inValue = new ClicWars.Api.ServerAdminService.ValidateAccountRequest();
            inValue.username = username;
            inValue.password = password;
            ClicWars.Api.ServerAdminService.ValidateAccountResponse retVal = ((ClicWars.Api.ServerAdminService.IServerAdminService)(this)).ValidateAccount(inValue);
            account = retVal.account;
            return retVal.ValidateAccountResult;
        }
        
        public System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.ValidateAccountResponse> ValidateAccountAsync(ClicWars.Api.ServerAdminService.ValidateAccountRequest request) {
            return base.Channel.ValidateAccountAsync(request);
        }
        
        public ClicWars.Api.ServerAdminService.AccountResult CreateAccount(string username, string password) {
            return base.Channel.CreateAccount(username, password);
        }
        
        public System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.AccountResult> CreateAccountAsync(string username, string password) {
            return base.Channel.CreateAccountAsync(username, password);
        }
        
        public ClicWars.Api.ServerAdminService.AccountResult GetAccount(string username) {
            return base.Channel.GetAccount(username);
        }
        
        public System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.AccountResult> GetAccountAsync(string username) {
            return base.Channel.GetAccountAsync(username);
        }
        
        public ClicWars.Api.ServerAdminService.CharacterResult CreateCharacter(string username, string name, int body) {
            return base.Channel.CreateCharacter(username, name, body);
        }
        
        public System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.CharacterResult> CreateCharacterAsync(string username, string name, int body) {
            return base.Channel.CreateCharacterAsync(username, name, body);
        }
        
        public ClicWars.Api.ServerAdminService.CharacterResult[] GetCharacters(string username) {
            return base.Channel.GetCharacters(username);
        }
        
        public System.Threading.Tasks.Task<ClicWars.Api.ServerAdminService.CharacterResult[]> GetCharactersAsync(string username) {
            return base.Channel.GetCharactersAsync(username);
        }
        
        public string CreateGameToken(string username, int serial) {
            return base.Channel.CreateGameToken(username, serial);
        }
        
        public System.Threading.Tasks.Task<string> CreateGameTokenAsync(string username, int serial) {
            return base.Channel.CreateGameTokenAsync(username, serial);
        }
    }
}