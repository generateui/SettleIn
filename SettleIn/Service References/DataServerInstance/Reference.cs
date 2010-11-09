﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SettleIn.DataServerInstance {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataServerInstance.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetAllUsers", ReplyAction="http://tempuri.org/IService1/GetAllUsersResponse")]
        SettleIn.DataServerInstance.GetAllUsersResponse GetAllUsers(SettleIn.DataServerInstance.GetAllUsersRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/GetAllUsers", ReplyAction="http://tempuri.org/IService1/GetAllUsersResponse")]
        System.IAsyncResult BeginGetAllUsers(SettleIn.DataServerInstance.GetAllUsersRequest request, System.AsyncCallback callback, object asyncState);
        
        SettleIn.DataServerInstance.GetAllUsersResponse EndGetAllUsers(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/IsUserNameTaken", ReplyAction="http://tempuri.org/IService1/IsUserNameTakenResponse")]
        SettleIn.DataServerInstance.IsUserNameTakenResponse IsUserNameTaken(SettleIn.DataServerInstance.IsUserNameTakenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/IsUserNameTaken", ReplyAction="http://tempuri.org/IService1/IsUserNameTakenResponse")]
        System.IAsyncResult BeginIsUserNameTaken(SettleIn.DataServerInstance.IsUserNameTakenRequest request, System.AsyncCallback callback, object asyncState);
        
        SettleIn.DataServerInstance.IsUserNameTakenResponse EndIsUserNameTaken(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/NewestUser", ReplyAction="http://tempuri.org/IService1/NewestUserResponse")]
        SettleIn.DataServerInstance.NewestUserResponse NewestUser(SettleIn.DataServerInstance.NewestUserRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/NewestUser", ReplyAction="http://tempuri.org/IService1/NewestUserResponse")]
        System.IAsyncResult BeginNewestUser(SettleIn.DataServerInstance.NewestUserRequest request, System.AsyncCallback callback, object asyncState);
        
        SettleIn.DataServerInstance.NewestUserResponse EndNewestUser(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/MostRecentGame", ReplyAction="http://tempuri.org/IService1/MostRecentGameResponse")]
        SettleIn.DataServerInstance.MostRecentGameResponse MostRecentGame(SettleIn.DataServerInstance.MostRecentGameRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/MostRecentGame", ReplyAction="http://tempuri.org/IService1/MostRecentGameResponse")]
        System.IAsyncResult BeginMostRecentGame(SettleIn.DataServerInstance.MostRecentGameRequest request, System.AsyncCallback callback, object asyncState);
        
        SettleIn.DataServerInstance.MostRecentGameResponse EndMostRecentGame(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Register", ReplyAction="http://tempuri.org/IService1/RegisterResponse")]
        SettleIn.DataServerInstance.RegisterResponse Register(SettleIn.DataServerInstance.RegisterRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/Register", ReplyAction="http://tempuri.org/IService1/RegisterResponse")]
        System.IAsyncResult BeginRegister(SettleIn.DataServerInstance.RegisterRequest request, System.AsyncCallback callback, object asyncState);
        
        SettleIn.DataServerInstance.RegisterResponse EndRegister(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetAllUsers", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetAllUsersRequest {
        
        public GetAllUsersRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetAllUsersResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetAllUsersResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Collections.Generic.List<SettleInCommon.User.XmlUser> GetAllUsersResult;
        
        public GetAllUsersResponse() {
        }
        
        public GetAllUsersResponse(System.Collections.Generic.List<SettleInCommon.User.XmlUser> GetAllUsersResult) {
            this.GetAllUsersResult = GetAllUsersResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IsUserNameTaken", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class IsUserNameTakenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string name;
        
        public IsUserNameTakenRequest() {
        }
        
        public IsUserNameTakenRequest(string name) {
            this.name = name;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IsUserNameTakenResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class IsUserNameTakenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool IsUserNameTakenResult;
        
        public IsUserNameTakenResponse() {
        }
        
        public IsUserNameTakenResponse(bool IsUserNameTakenResult) {
            this.IsUserNameTakenResult = IsUserNameTakenResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="NewestUser", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class NewestUserRequest {
        
        public NewestUserRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="NewestUserResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class NewestUserResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public SettleInCommon.User.XmlUser NewestUserResult;
        
        public NewestUserResponse() {
        }
        
        public NewestUserResponse(SettleInCommon.User.XmlUser NewestUserResult) {
            this.NewestUserResult = NewestUserResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MostRecentGame", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class MostRecentGameRequest {
        
        public MostRecentGameRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MostRecentGameResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class MostRecentGameResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public SettleInCommon.Gaming.XmlGameResult MostRecentGameResult;
        
        public MostRecentGameResponse() {
        }
        
        public MostRecentGameResponse(SettleInCommon.Gaming.XmlGameResult MostRecentGameResult) {
            this.MostRecentGameResult = MostRecentGameResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Register", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RegisterRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string pasword;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string email;
        
        public RegisterRequest() {
        }
        
        public RegisterRequest(string name, string pasword, string email) {
            this.name = name;
            this.pasword = pasword;
            this.email = email;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RegisterResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RegisterResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public SettleInCommon.User.XmlUser RegisterResult;
        
        public RegisterResponse() {
        }
        
        public RegisterResponse(SettleInCommon.User.XmlUser RegisterResult) {
            this.RegisterResult = RegisterResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : SettleIn.DataServerInstance.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetAllUsersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetAllUsersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SettleIn.DataServerInstance.GetAllUsersResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SettleIn.DataServerInstance.GetAllUsersResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IsUserNameTakenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public IsUserNameTakenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SettleIn.DataServerInstance.IsUserNameTakenResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SettleIn.DataServerInstance.IsUserNameTakenResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NewestUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public NewestUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SettleIn.DataServerInstance.NewestUserResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SettleIn.DataServerInstance.NewestUserResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MostRecentGameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public MostRecentGameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SettleIn.DataServerInstance.MostRecentGameResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SettleIn.DataServerInstance.MostRecentGameResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RegisterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SettleIn.DataServerInstance.RegisterResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SettleIn.DataServerInstance.RegisterResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<SettleIn.DataServerInstance.IService1>, SettleIn.DataServerInstance.IService1 {
        
        private BeginOperationDelegate onBeginGetAllUsersDelegate;
        
        private EndOperationDelegate onEndGetAllUsersDelegate;
        
        private System.Threading.SendOrPostCallback onGetAllUsersCompletedDelegate;
        
        private BeginOperationDelegate onBeginIsUserNameTakenDelegate;
        
        private EndOperationDelegate onEndIsUserNameTakenDelegate;
        
        private System.Threading.SendOrPostCallback onIsUserNameTakenCompletedDelegate;
        
        private BeginOperationDelegate onBeginNewestUserDelegate;
        
        private EndOperationDelegate onEndNewestUserDelegate;
        
        private System.Threading.SendOrPostCallback onNewestUserCompletedDelegate;
        
        private BeginOperationDelegate onBeginMostRecentGameDelegate;
        
        private EndOperationDelegate onEndMostRecentGameDelegate;
        
        private System.Threading.SendOrPostCallback onMostRecentGameCompletedDelegate;
        
        private BeginOperationDelegate onBeginRegisterDelegate;
        
        private EndOperationDelegate onEndRegisterDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterCompletedDelegate;
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<GetAllUsersCompletedEventArgs> GetAllUsersCompleted;
        
        public event System.EventHandler<IsUserNameTakenCompletedEventArgs> IsUserNameTakenCompleted;
        
        public event System.EventHandler<NewestUserCompletedEventArgs> NewestUserCompleted;
        
        public event System.EventHandler<MostRecentGameCompletedEventArgs> MostRecentGameCompleted;
        
        public event System.EventHandler<RegisterCompletedEventArgs> RegisterCompleted;
        
        public SettleIn.DataServerInstance.GetAllUsersResponse GetAllUsers(SettleIn.DataServerInstance.GetAllUsersRequest request) {
            return base.Channel.GetAllUsers(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetAllUsers(SettleIn.DataServerInstance.GetAllUsersRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetAllUsers(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public SettleIn.DataServerInstance.GetAllUsersResponse EndGetAllUsers(System.IAsyncResult result) {
            return base.Channel.EndGetAllUsers(result);
        }
        
        private System.IAsyncResult OnBeginGetAllUsers(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SettleIn.DataServerInstance.GetAllUsersRequest request = ((SettleIn.DataServerInstance.GetAllUsersRequest)(inValues[0]));
            return this.BeginGetAllUsers(request, callback, asyncState);
        }
        
        private object[] OnEndGetAllUsers(System.IAsyncResult result) {
            SettleIn.DataServerInstance.GetAllUsersResponse retVal = this.EndGetAllUsers(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetAllUsersCompleted(object state) {
            if ((this.GetAllUsersCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetAllUsersCompleted(this, new GetAllUsersCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetAllUsersAsync(SettleIn.DataServerInstance.GetAllUsersRequest request) {
            this.GetAllUsersAsync(request, null);
        }
        
        public void GetAllUsersAsync(SettleIn.DataServerInstance.GetAllUsersRequest request, object userState) {
            if ((this.onBeginGetAllUsersDelegate == null)) {
                this.onBeginGetAllUsersDelegate = new BeginOperationDelegate(this.OnBeginGetAllUsers);
            }
            if ((this.onEndGetAllUsersDelegate == null)) {
                this.onEndGetAllUsersDelegate = new EndOperationDelegate(this.OnEndGetAllUsers);
            }
            if ((this.onGetAllUsersCompletedDelegate == null)) {
                this.onGetAllUsersCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetAllUsersCompleted);
            }
            base.InvokeAsync(this.onBeginGetAllUsersDelegate, new object[] {
                        request}, this.onEndGetAllUsersDelegate, this.onGetAllUsersCompletedDelegate, userState);
        }
        
        public SettleIn.DataServerInstance.IsUserNameTakenResponse IsUserNameTaken(SettleIn.DataServerInstance.IsUserNameTakenRequest request) {
            return base.Channel.IsUserNameTaken(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginIsUserNameTaken(SettleIn.DataServerInstance.IsUserNameTakenRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginIsUserNameTaken(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public SettleIn.DataServerInstance.IsUserNameTakenResponse EndIsUserNameTaken(System.IAsyncResult result) {
            return base.Channel.EndIsUserNameTaken(result);
        }
        
        private System.IAsyncResult OnBeginIsUserNameTaken(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SettleIn.DataServerInstance.IsUserNameTakenRequest request = ((SettleIn.DataServerInstance.IsUserNameTakenRequest)(inValues[0]));
            return this.BeginIsUserNameTaken(request, callback, asyncState);
        }
        
        private object[] OnEndIsUserNameTaken(System.IAsyncResult result) {
            SettleIn.DataServerInstance.IsUserNameTakenResponse retVal = this.EndIsUserNameTaken(result);
            return new object[] {
                    retVal};
        }
        
        private void OnIsUserNameTakenCompleted(object state) {
            if ((this.IsUserNameTakenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.IsUserNameTakenCompleted(this, new IsUserNameTakenCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void IsUserNameTakenAsync(SettleIn.DataServerInstance.IsUserNameTakenRequest request) {
            this.IsUserNameTakenAsync(request, null);
        }
        
        public void IsUserNameTakenAsync(SettleIn.DataServerInstance.IsUserNameTakenRequest request, object userState) {
            if ((this.onBeginIsUserNameTakenDelegate == null)) {
                this.onBeginIsUserNameTakenDelegate = new BeginOperationDelegate(this.OnBeginIsUserNameTaken);
            }
            if ((this.onEndIsUserNameTakenDelegate == null)) {
                this.onEndIsUserNameTakenDelegate = new EndOperationDelegate(this.OnEndIsUserNameTaken);
            }
            if ((this.onIsUserNameTakenCompletedDelegate == null)) {
                this.onIsUserNameTakenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnIsUserNameTakenCompleted);
            }
            base.InvokeAsync(this.onBeginIsUserNameTakenDelegate, new object[] {
                        request}, this.onEndIsUserNameTakenDelegate, this.onIsUserNameTakenCompletedDelegate, userState);
        }
        
        public SettleIn.DataServerInstance.NewestUserResponse NewestUser(SettleIn.DataServerInstance.NewestUserRequest request) {
            return base.Channel.NewestUser(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginNewestUser(SettleIn.DataServerInstance.NewestUserRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginNewestUser(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public SettleIn.DataServerInstance.NewestUserResponse EndNewestUser(System.IAsyncResult result) {
            return base.Channel.EndNewestUser(result);
        }
        
        private System.IAsyncResult OnBeginNewestUser(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SettleIn.DataServerInstance.NewestUserRequest request = ((SettleIn.DataServerInstance.NewestUserRequest)(inValues[0]));
            return this.BeginNewestUser(request, callback, asyncState);
        }
        
        private object[] OnEndNewestUser(System.IAsyncResult result) {
            SettleIn.DataServerInstance.NewestUserResponse retVal = this.EndNewestUser(result);
            return new object[] {
                    retVal};
        }
        
        private void OnNewestUserCompleted(object state) {
            if ((this.NewestUserCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.NewestUserCompleted(this, new NewestUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void NewestUserAsync(SettleIn.DataServerInstance.NewestUserRequest request) {
            this.NewestUserAsync(request, null);
        }
        
        public void NewestUserAsync(SettleIn.DataServerInstance.NewestUserRequest request, object userState) {
            if ((this.onBeginNewestUserDelegate == null)) {
                this.onBeginNewestUserDelegate = new BeginOperationDelegate(this.OnBeginNewestUser);
            }
            if ((this.onEndNewestUserDelegate == null)) {
                this.onEndNewestUserDelegate = new EndOperationDelegate(this.OnEndNewestUser);
            }
            if ((this.onNewestUserCompletedDelegate == null)) {
                this.onNewestUserCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnNewestUserCompleted);
            }
            base.InvokeAsync(this.onBeginNewestUserDelegate, new object[] {
                        request}, this.onEndNewestUserDelegate, this.onNewestUserCompletedDelegate, userState);
        }
        
        public SettleIn.DataServerInstance.MostRecentGameResponse MostRecentGame(SettleIn.DataServerInstance.MostRecentGameRequest request) {
            return base.Channel.MostRecentGame(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginMostRecentGame(SettleIn.DataServerInstance.MostRecentGameRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginMostRecentGame(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public SettleIn.DataServerInstance.MostRecentGameResponse EndMostRecentGame(System.IAsyncResult result) {
            return base.Channel.EndMostRecentGame(result);
        }
        
        private System.IAsyncResult OnBeginMostRecentGame(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SettleIn.DataServerInstance.MostRecentGameRequest request = ((SettleIn.DataServerInstance.MostRecentGameRequest)(inValues[0]));
            return this.BeginMostRecentGame(request, callback, asyncState);
        }
        
        private object[] OnEndMostRecentGame(System.IAsyncResult result) {
            SettleIn.DataServerInstance.MostRecentGameResponse retVal = this.EndMostRecentGame(result);
            return new object[] {
                    retVal};
        }
        
        private void OnMostRecentGameCompleted(object state) {
            if ((this.MostRecentGameCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.MostRecentGameCompleted(this, new MostRecentGameCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void MostRecentGameAsync(SettleIn.DataServerInstance.MostRecentGameRequest request) {
            this.MostRecentGameAsync(request, null);
        }
        
        public void MostRecentGameAsync(SettleIn.DataServerInstance.MostRecentGameRequest request, object userState) {
            if ((this.onBeginMostRecentGameDelegate == null)) {
                this.onBeginMostRecentGameDelegate = new BeginOperationDelegate(this.OnBeginMostRecentGame);
            }
            if ((this.onEndMostRecentGameDelegate == null)) {
                this.onEndMostRecentGameDelegate = new EndOperationDelegate(this.OnEndMostRecentGame);
            }
            if ((this.onMostRecentGameCompletedDelegate == null)) {
                this.onMostRecentGameCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnMostRecentGameCompleted);
            }
            base.InvokeAsync(this.onBeginMostRecentGameDelegate, new object[] {
                        request}, this.onEndMostRecentGameDelegate, this.onMostRecentGameCompletedDelegate, userState);
        }
        
        public SettleIn.DataServerInstance.RegisterResponse Register(SettleIn.DataServerInstance.RegisterRequest request) {
            return base.Channel.Register(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginRegister(SettleIn.DataServerInstance.RegisterRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegister(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public SettleIn.DataServerInstance.RegisterResponse EndRegister(System.IAsyncResult result) {
            return base.Channel.EndRegister(result);
        }
        
        private System.IAsyncResult OnBeginRegister(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SettleIn.DataServerInstance.RegisterRequest request = ((SettleIn.DataServerInstance.RegisterRequest)(inValues[0]));
            return this.BeginRegister(request, callback, asyncState);
        }
        
        private object[] OnEndRegister(System.IAsyncResult result) {
            SettleIn.DataServerInstance.RegisterResponse retVal = this.EndRegister(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRegisterCompleted(object state) {
            if ((this.RegisterCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterCompleted(this, new RegisterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterAsync(SettleIn.DataServerInstance.RegisterRequest request) {
            this.RegisterAsync(request, null);
        }
        
        public void RegisterAsync(SettleIn.DataServerInstance.RegisterRequest request, object userState) {
            if ((this.onBeginRegisterDelegate == null)) {
                this.onBeginRegisterDelegate = new BeginOperationDelegate(this.OnBeginRegister);
            }
            if ((this.onEndRegisterDelegate == null)) {
                this.onEndRegisterDelegate = new EndOperationDelegate(this.OnEndRegister);
            }
            if ((this.onRegisterCompletedDelegate == null)) {
                this.onRegisterCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterDelegate, new object[] {
                        request}, this.onEndRegisterDelegate, this.onRegisterCompletedDelegate, userState);
        }
    }
}