using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup.Installing
{
    internal class WinUserCollection : IReadOnlyList<WinUser>
    {
        private readonly WinUser[] Users;

        public WinUser this[int index] => Users[index];

        public int Count => Users.Length;

        public WinUserCollection(WinUser[] users)
        {
            Users = users;
        }

        public static WinUserCollection Create()
        {
            string usersDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "..");
            string[] userDirectories = Directory.GetDirectories(usersDirectory);
            WinUser[] users = userDirectories.Select(x => new WinUser(Path.GetFileName(x), x)).ToArray();

            return new WinUserCollection(users);
        }

        public IEnumerator<WinUser> GetEnumerator() => ((IEnumerable<WinUser>)Users).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Users.GetEnumerator();
    }
}
