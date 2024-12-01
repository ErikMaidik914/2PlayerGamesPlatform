using System;
using _2PlayerGames;

namespace _2PlayerGames
{
    public class Sounds
    {
        private Guid _id;
        private string _name;
        private string _category;
        private string _path;

        public string Name { get => _name; set => _name = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Category { get => _category; set => _category = value; }

        public string Path { get => _path; set => _path = value; }
        //sound class
        public Sounds(string name, string category, string path)
        {
            this._id = Guid.NewGuid();
            this._name = name;
            this._category = category;
            this._path = path;
        }

        public Sounds(Guid id, string name, string category, string path)
        {
            this._id = id;
            this._name = name;
            this._category = category;
            this._path = path;
        }

        public Sounds()
        {
            this._id = Guid.Empty;
            this._name = "";
            this._category = "";
            this._path = "";
        }
    }
}
