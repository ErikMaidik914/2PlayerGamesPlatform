using System;
using _2PlayerGames;
using _2PlayerGames.domain.Bot;

namespace _2PlayerGames
{
    [Serializable]
    public class Player 
    {
        private Guid _id;
        private string _name;
        private string? _ip;
        private int? _port;

        public string Name { get => _name; set => _name = value; }
        public Guid Id { get => _id; set => _id = value; }
        public string? Ip { get => _ip; set => _ip = value; }
        public int? Port { get => _port; set => _port = value; }

        public Player(string name, string? ip, int? port)
        {
            this._id = Guid.NewGuid();
            this._name = name;
            this._ip = ip;
            this._port = port;;
        }

        public Player(Guid id, string name, string? ip, int? port)
        {
            this._id = id;
            this._name = name;
            this._ip = ip;
            this._port = port;
        }

        public Player() {
            this._id = Guid.Empty;
            this._name = "";
            this._ip = "";
            this._port = 0;
        }

        public override string ToString()
        {
            return this._name;
        }

        public (int nrParameters, object[] parameters) Play(IGame newGame)
        {
            throw new NotImplementedException();
        }

        public static Player Null()
        {
            return new Player(Guid.Empty, "Null", "Null", 0);
        }

        public static Player Bot()
        {
            return new Player(Guid.Empty, "Bot", "Null", 0);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Player p = (Player)obj;
            return this._id == p.Id;
        }


    }
}