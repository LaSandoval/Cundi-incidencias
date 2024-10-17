namespace Cundi_incidencias.Utility
{
    public class CodigoRecuperacionUtility
    {
        public int NumeroAleatorio()
        {
            Random r = new Random();
            return r.Next(100000, 999999 + 1);
        }
    }
}
