using Mono.Cecil;

namespace WurstMod.Manglers
{
    public class UnityAssemblyResolver : BaseAssemblyResolver
    {
        private DefaultAssemblyResolver defaultResolver = new DefaultAssemblyResolver();
        private string managed;

        public UnityAssemblyResolver(string pathToManaged)
        {
            managed = pathToManaged;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            AssemblyDefinition asm;
            try
            {
                asm = defaultResolver.Resolve(name);
            }
            catch (AssemblyResolutionException)
            {
                ReaderParameters readerParams = new ReaderParameters();
                readerParams.AssemblyResolver = this;
                asm = AssemblyDefinition.ReadAssembly(managed + name.Name + ".dll", readerParams);
            }

            return asm;
        }
    }
}