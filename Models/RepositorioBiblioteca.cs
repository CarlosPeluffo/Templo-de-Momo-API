using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioBiblioteca : Base, IRepositorioBiblioteca
    {
        public RepositorioBiblioteca(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Biblioteca biblioteca){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO bibliotecas(JuegoId, UsuarioId, fecha_seguido)
                    VALUES(@juego, @usuario, @fecha);
                    SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@juego", biblioteca.JuegoId);
                    comm.Parameters.AddWithValue("@usuario", biblioteca.UsuarioId);
                    comm.Parameters.AddWithValue("@fecha", biblioteca.FechaSeguido);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                }
            }
            return res;
        }
        public int Baja(Biblioteca biblioteca){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM usuarios WHERE id_usuario=@idU AND id_juego=@idJ";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@idU", biblioteca.UsuarioId);
                    comm.Parameters.AddWithValue("@idJ", biblioteca.JuegoId);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Biblioteca biblioteca){
            int res = -1;
            return res;
        }
        public IList<Biblioteca> ObtenerTodos(){
            IList<Biblioteca> lista = new List<Biblioteca>();
            return lista;
        }
        public Biblioteca ObtenerPorId(int id){
            Biblioteca b = null;
            return b;
        }
        public IList<Biblioteca> ObtenerPorUsuario(int id){
            IList<Biblioteca> lista = new List<Biblioteca>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT JuegoId, UsuarioId, fecha_seguido
                    FROM bibliotecas
                    WHERE UsuarioId=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Biblioteca b = new Biblioteca{
                            JuegoId = reader.GetInt32(0),
                            UsuarioId = reader.GetInt32(1),
                            FechaSeguido = reader.GetDateTime(2)
                        };
                        lista.Add(b);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
    }
}