using dominio;

namespace negocio
{
    public static class Seguridad
    {
        public static bool sesionActiva(object usuario)
        {
            return usuario != null;
        }

        public static bool esAdmin(object usuario)
        {
            Usuarios user = usuario as Usuarios;
            return user != null && user.Perfil.Nombre == "Administrador";
        }

        public static bool esSupervisor(object usuario)
        {
            Usuarios user = usuario as Usuarios;
            return user != null && user.Perfil.Nombre == "Supervisor";
        }

        public static bool esTelefonista(object usuario)
        {
            Usuarios user = usuario as Usuarios;
            return user != null && user.Perfil.Nombre == "Telefonista";
        }

        public static bool puedeVerIncidencia(object usuario, int idAsignado)
        {
            Usuarios user = usuario as Usuarios;
            return esAdmin(user) || esSupervisor(user) || (esTelefonista(user) && user.IDUsuario == idAsignado);
        }
    }
}