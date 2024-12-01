using System;

namespace _2PlayerGames
{
    public class Games
    {
        private Guid _id;
        private string _name;
        private string _category;

        public string Name { get => _name; set => _name = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string Category { get => _category; set => _category = value; }

        public Games(string name, string category)
        {
            this._id = Guid.NewGuid();
            this._name = name;
            this._category = category;
        }

        public Games(Guid id, string name, string category)
        {
            this._id = id;
            this._name = name;
            this._category = category;
        }

        public Games()
        {
            this._id = Guid.Empty;
            this._name = "";
            this._category = "";
        }
    }
}