using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioNoticia : Base, IRepositorioNoticia
    {
        public RepositorioNoticia(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Noticia noticia){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO noticias(CreadorId, JuegoId, fecha, titulo, cuerpo)
                    VALUES(@creador, @juego, NOW(), @titulo, @cuerpo);
                    SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@creador", noticia.CreadorId);
                    comm.Parameters.AddWithValue("@juego", noticia.JuegoId);
                    comm.Parameters.AddWithValue("@titulo", noticia.Titulo);
                    comm.Parameters.AddWithValue("@cuerpo", noticia.Cuerpo);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    noticia.Id = res;
                }
            }
            return res;
        }
        public int Baja(Noticia noticia){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM noticias WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", noticia.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Noticia noticia){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE noticias
                    SET CreadorId=@creador, JuegoId=@juego,
                    titulo=@titulo, cuerpo=@cuerpo
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@creador", noticia.CreadorId);
                    comm.Parameters.AddWithValue("@juego", noticia.JuegoId);
                    comm.Parameters.AddWithValue("@titulo", noticia.Titulo);
                    comm.Parameters.AddWithValue("@cuerpo", noticia.Cuerpo);
                    comm.Parameters.AddWithValue("@id", noticia.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Noticia> ObtenerTodos(){
            IList<Noticia> lista = new List<Noticia>();
            using(MySqlConnection conn= new MySqlConnection(connectionString)){
                string sql = @"SELECT n.id, n.CreadorId, n.JuegoId, fecha, n.titulo,
                    cuerpo, j.titulo, c.nick_name
                    FROM noticias n
                    INNER JOIN creadores c ON n.CreadorId = c.Id
                    INNER JOIN juegos j ON n.JuegoId = j.Id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Noticia noticia= new Noticia{
                            Id = reader.GetInt32(0),
                            CreadorId = reader.GetInt32(1),
                            JuegoId = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Titulo = reader.GetString(4),
                            Cuerpo = reader.GetString(5),
                            Creador = new Creador{
                                Id = reader.GetInt32(1),
                                NickName = reader.GetString(7)
                            },
                            Juego = new Juego{
                                Id = reader.GetInt32(2),
                                Titulo = reader.GetString(6)
                            }
                        };
                        lista.Add(noticia);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Noticia ObtenerPorId(int id){
            Noticia noticia = null;
            using(MySqlConnection conn= new MySqlConnection(connectionString)){
                string sql = @"SELECT n.id, CreadorId, JuegoId, fecha, titulo, 
                    cuerpo, j.titulo, c.nick_name
                    FROM noticias n
                    INNER JOIN creadores c ON n.CreadorId = c.Id
                    INNER JOIN juegos j ON n,JuegoId = j.Id
                    WHERE n.id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        noticia= new Noticia{
                            Id = reader.GetInt32(0),
                            CreadorId = reader.GetInt32(1),
                            JuegoId = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Titulo = reader.GetString(4),
                            Cuerpo = reader.GetString(5),
                            Creador = new Creador{
                                Id = reader.GetInt32(1),
                                NickName = reader.GetString(7)
                            },
                            Juego = new Juego{
                                Id = reader.GetInt32(2),
                                Titulo = reader.GetString(6)
                            }
                        };
                    }
                    conn.Close();
                }
            }
            return noticia;
        }
    }
}