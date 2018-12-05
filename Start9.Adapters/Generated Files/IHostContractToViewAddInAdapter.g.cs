//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Start9.Api.AddInSideAdapters
{
    
    public class IHostContractToViewAddInAdapter : Start9.Api.IHost
    {
        private Start9.Api.Contracts.IHostContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static IHostContractToViewAddInAdapter()
        {
        }
        public IHostContractToViewAddInAdapter(Start9.Api.Contracts.IHostContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public void PushMessage(Start9.Api.IMessage message)
        {
            _contract.PushMessage(Start9.Api.AddInSideAdapters.IMessageAddInAdapter.ViewToContractAdapter(message));
        }
        public System.Collections.Generic.IList<Start9.Api.IConfigurationEntry> GetConfigurationEntries(Start9.Api.IConfiguration configuration)
        {
            return System.AddIn.Pipeline.CollectionAdapters.ToIList<Start9.Api.Contracts.IConfigurationEntryContract, Start9.Api.IConfigurationEntry>(_contract.GetConfigurationEntries(Start9.Api.AddInSideAdapters.IConfigurationAddInAdapter.ViewToContractAdapter(configuration)), Start9.Api.AddInSideAdapters.IConfigurationEntryAddInAdapter.ContractToViewAdapter, Start9.Api.AddInSideAdapters.IConfigurationEntryAddInAdapter.ViewToContractAdapter);
        }
        public Start9.Api.IConfiguration GetGlobalConfiguration()
        {
            return Start9.Api.AddInSideAdapters.IConfigurationAddInAdapter.ContractToViewAdapter(_contract.GetGlobalConfiguration());
        }
        public System.Collections.Generic.IList<Start9.Api.IModule> GetInstancesOfExecutingModule()
        {
            return System.AddIn.Pipeline.CollectionAdapters.ToIList<Start9.Api.Contracts.IModuleContract, Start9.Api.IModule>(_contract.GetInstancesOfExecutingModule(), Start9.Api.AddInSideAdapters.IModuleAddInAdapter.ContractToViewAdapter, Start9.Api.AddInSideAdapters.IModuleAddInAdapter.ViewToContractAdapter);
        }
        internal Start9.Api.Contracts.IHostContract GetSourceContract()
        {
            return _contract;
        }
    }
}
