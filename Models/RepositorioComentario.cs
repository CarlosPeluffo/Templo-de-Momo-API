using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioComentario : Base, IRepositorioComentario
    {
        public RepositorioComentario(IConfiguration configuration) : base(configuration)
        {
            
        }

        public int Alta(Comentario comentario){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO comentarios(NoticiaId, UsuarioId, fecha, cuerpo)
                    VALUES(@noticia, @usuario, NOW(), @cuerpo);
                    SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@noticia", comentario.NoticiaId);
                    comm.Parameters.AddWithValue("@usuario", comentario.UsuarioId);
                    comm.Parameters.AddWithValue("@cuerpo", comentario.Cuerpo);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    comentario.Id = res;
                }
            }
            return res;
        }
        public int Baja(Comentario comentario){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM comentarios WHERE id_usuario=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", comentario.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Comentario comentario){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE comentarios
                    SET NoticiaId=@noticia, UsuarioId=@usuario, cuerpo=@cuerpo
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@noticia", comentario.NoticiaId);
                    comm.Parameters.AddWithValue("@usuario", comentario.UsuarioId);
                    comm.Parameters.AddWithValue("@cuerpo", comentario.Cuerpo);
                }
            }
            return res;
        }
        public IList<Comentario> ObtenerTodos(){
            IList<Comentario> lista = new List<Comentario>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT c.id, NoticiaId, UsuarioId, c.fecha, c.cuerpo,
                    s.nick_name, s.avatar
                    FROM comentarios c
                    INNER JOIN usuarios s ON c.UsuarioId = s.Id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Comentario comentario = new Comentario{
                            Id = reader.GetInt32(0),
                            NoticiaId = reader.GetInt32(1),
                            UsuarioId = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Cuerpo = reader.GetString(4),
                            Usuario = new Usuario{
                                Id = reader.GetInt32(2),
                                NickName = reader.GetString(5),
                                Avatar = reader.GetString(6)
                            }
                        };
                        lista.Add(comentario);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Comentario ObtenerPorId(int id){
            Comentario comentario = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT c.id, NoticiaId, UsuarioId, c.fecha, c.cuerpo
                    s.nick_name, s.avatar
                    FROM comentarios c
                    INNER JOIN usuarios s ON c.UsuarioId = s.Id
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        comentario = new Comentario{
                            Id = reader.GetInt32(0),
                            NoticiaId = reader.GetInt32(1),
                            UsuarioId = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Cuerpo = reader.GetString(4),
                            Usuario = new Usuario{
                                Id = reader.GetInt32(2),
                                NickName = reader.GetString(5),
                                Avatar = reader.GetString(6)
                            }
                        };
                    }
                    conn.Close();
                }
            }
            return comentario;
        }
        public IList<Comentario> ObtenerPorNoticia(int id){
            IList<Comentario> lista = new List<Comentario>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT c.id, NoticiaId, UsuarioId, c.fecha, c.cuerpo,
                    s.nick_name, s.avatar
                    FROM comentarios c
                    INNER JOIN usuarios s ON c.UsuarioId = s.Id
                    WHERE c.NoticiaId=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Comentario comentario = new Comentario{
                            Id = reader.GetInt32(0),
                            NoticiaId = reader.GetInt32(1),
                            UsuarioId = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Cuerpo = reader.GetString(4),
                            Usuario = new Usuario{
                                Id = reader.GetInt32(2),
                                NickName = reader.GetString(5),
                                Avatar = reader.GetString(6)
                            }
                        };
                        lista.Add(comentario);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
    }
}