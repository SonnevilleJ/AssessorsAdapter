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

        public HouseApp()
            : this(new AssessorsRepository())
        {
        }

        public HouseApp(string path)
            : this(new CachingRepository<string, IHouse>(new AssessorsRepository(), new XmlRepository<IHouse>(path)))
        {
        }

        public HouseApp(IRepository<string, IHouse> repository)
        {
            Repository = repository;
        }

        public IHouse GetHouse(string address)
        {
            return Repository.Fetch(address);
        }
    }
}