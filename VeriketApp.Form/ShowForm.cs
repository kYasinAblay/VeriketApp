

using Nito.AsyncEx;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VeriketApp.Form
{
    public partial class ShowForm : System.Windows.Forms.Form
    {
        ConcurrentBag<string> list;
        public ShowForm()
        {
            InitializeComponent();
            //consumerProducerLines.WriteLogEvent += addItemToListBox;
        }
        
        private void btnLog_Click(object sender, EventArgs e)
        {
            list = new ConcurrentBag<string>();
            string parentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows)[0].ToString();
            var lines = AsyncContext.Run(async () =>
            {
                await Task.Delay(1500);
                using (var reader = File.OpenText(string.Format("{0}:\\ProgramData\\VeriketApp\\Log.txt", parentFolder)))
                {
                    var fileText = await reader.ReadToEndAsync();
                    return fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                }
            });


            list_Log.Items.Clear();
            foreach (var line in lines)
                list.Add(line);
            foreach (var item in list)
                list_Log.Items.Add(item);

        }

        //public void addItemToListBox(string item)
        //{
        //    this.Invoke(() =>
        //    {
        //        list_Log.Invoke((MethodInvoker)delegate
        //        {
        //            list_Log.Items.Add(item);
        //        });
        //    });
        //}
    }
}
