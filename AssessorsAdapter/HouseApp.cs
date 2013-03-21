using System.Runtime.InteropServices;
using Sonneville.Utilities.Persistence;

namespace AssessorsAdapter
{
#if DEBUG
    [ClassInterface(ClassInterfaceType.AutoDual)]
#endif
    [ComVisible(true)]
    public class HouseApp
    {
        [ComVisible(false)]
        public IRepository<string, IHouse> Repository { get; private set; }

        public void Initialize()
        {
            Initialize_3(new AssessorsRepository());
        }

        public void Initialize_2(string path)
        {
            Initialize_3(new CachingRepository<string, IHouse>(new AssessorsRepository(), new XmlRepository<IHouse>(path)));
        }

        public void Initialize_3(IRepository<string, IHouse> repository)
        {
            Repository = repository;
        }

        public IHouse GetHouse(string address)
        {
            return Repository.Fetch(address);
        }
    }
}