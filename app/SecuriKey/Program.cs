
namespace SecuriKey
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new mainForm());
            Application.Run(new SecuriKeyForm());
        }
    }

}