//using Nito.AsyncEx;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace VeriketApp.Form
//{
//    public class ConsumerProducerLines
//    {
//        public delegate void WriteLogHandler(string log);
//        public  event WriteLogHandler WriteLogEvent;
//        public ConsumerProducerLines()
//        {
           
//        }

//        static BlockingCollection<string> Logs = new BlockingCollection<string>(
//          new ConcurrentBag<string>(), 10 /* bounded */
//        );

//        static CancellationTokenSource cts = new CancellationTokenSource();

//        public void ProduceAndConsume()
//        {
//            var producer = Task.Factory.StartNew(RunProducer);
//            var consumer = Task.Factory.StartNew(RunConsumer);

//            try
//            {
//                Task.WaitAll(new[] { producer, consumer }, cts.Token);
//            }
//            catch (AggregateException ae)
//            {
//                ae.Handle(e =>
//                {
//                    Console.WriteLine("{0} message: [{1}]", e.StackTrace, e.Message);
//                    return true;
//                });
//            }
//        }

//        private  void RunConsumer()
//        {
//            foreach (var item in Logs.GetConsumingEnumerable())
//            {
//                cts.Token.ThrowIfCancellationRequested();
//                if (WriteLogEvent != null)
//                {
//                    WriteLogEvent(item);
//                }
//            }
//        }

//        private  void RunProducer()
//        {

//            while (true)
//            {
//                cts.Token.ThrowIfCancellationRequested();

//                string parentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows)[0].ToString();
//                var lines = AsyncContext.Run(async () =>
//                {
//                    using (var reader = File.OpenText(string.Format("{0}:\\ProgramData\\VeriketApp\\Log.txt", parentFolder)))
//                    {
//                        var fileText = await reader.ReadToEndAsync();
//                        return fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
//                    }
//                });

//                foreach (var line in lines)
//                    Logs.Add(line);
//            }
//        }
//    }
//}
