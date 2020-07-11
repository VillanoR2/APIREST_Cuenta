using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICuentas_Practica.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static APICuentas_Practica.Models.ServiciosDeEncriptacion;

namespace APICuentas_Practica.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> Usuarios;

        public UserService(IUsuarioDatabaseSettings settings)
        {
            var client = new MongoClient("mongodb+srv://root:rootpass@db.f6psa.mongodb.net/CuentaDB?retryWrites=true&w=majority");
            var database = client.GetDatabase(settings.DatabaseName);

            Usuarios = database.GetCollection<User>(settings.UsuariosCollectionName);
        }

        public List<User> Get()
        {
            List<User> usuarios = new List<User>();
            try
            {
                usuarios = Usuarios.Find(usuario => true).ToList();
            }
            catch (TimeoutException e)
            {
                usuarios.Add(new User()
                {
                    Id = "12",
                    Username = "DatabaseCaida",
                    Password = "pass",
                    Access = false
                });
            }
            return usuarios;
        }


        public User Get(string id) =>
            Usuarios.Find<User>(usuario => usuario.Id == id).FirstOrDefault();

        public User Create(User usuario)
        {
            usuario.Id = string.Empty;
            usuario.Password = EncriptarCadena(usuario.Password);
            Usuarios.InsertOne(usuario);
            return usuario;
        }

        public void Update(string id, User usuarioActualizar) =>
            Usuarios.ReplaceOne(usuario => usuario.Id == id, usuarioActualizar);

        public void Remove(User usuarioActualizar) =>
            Usuarios.DeleteOne(usuario => usuario.Id == usuarioActualizar.Id);

        public void Remove(string id) =>
            Usuarios.DeleteOne(usuario => usuario.Id == id);

    }
}