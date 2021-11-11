using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;

namespace ServicoJainaC
{
    [RunInstaller(true)]
    public partial class ServicoJainaC : ServiceBase
    {
        int Tempo = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadSleepTimeInMin"]);
        public Thread worker = null;
        int vezes = 0;
        String path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() +"arquivo.txt";
        public ServicoJainaC()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format("Iniciando o serviço"));
                    writer.Close();
                }
                ThreadStart start = new ThreadStart(Working);
                worker = new Thread(start);
                worker.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Working()
        {
            while (true)
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format("Se passou 1 segundo..."));
                    writer.Close();
                }
                Thread.Sleep(1000);
            }
        }
        public void onDebug()
        {
            OnStart(null);
        }
        protected override void OnStop()
        {
            try
            {
                if ((worker != null) & worker.IsAlive)
                {
                    worker.Abort();
                }
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format("Parando o serviço"));
                    writer.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
