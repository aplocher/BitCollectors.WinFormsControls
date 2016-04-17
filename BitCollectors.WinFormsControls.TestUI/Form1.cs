using System;
using System.Windows.Forms;
using BitCollectors.WinFormsControls;

namespace BitCollectors.WinFormsControl.TestUI
{
    public partial class Form1 : Form
    {
        private const int _loopCount = 1000;

        private bool _bgWorkerDone = false;
        private Random _randomNameNumber = new Random();
        private int _threadCount = 0;

        private string[] _firstNames =
        {
            "Greg", "Adam", "Alejandra", "Jay", "Mike", "Michael", "James", "Dennis", "Jim", "Jerry", "Sharon", "Nancy", "Bill", "George", "Abraham", "Barack",
            "Chief", "Smokey", "Walter", "Marlon", "Denny", "Daniel", "Dan", "Mary", "Marie", "Fred", "Bob", "iiiAfirst", "lllFirst", "il1MOP", "Joseph"
        };

        private string[] _lastNames =
        {
            "Smith", "Plocher", "Herrera", "Hopper", "Navarro", "Bolar", "Williams", "Johnson", "Jones", "Brown",
            "David", "Miller", "Taylor", "Jackson", "White", "Clark", "Rodriguez", "il1MOP", "iiiAlast", "lllLast", "Frederickson", "Obama", "Clinton", "Bush", "Bin Laden", "Stalin"
        };

        public Form1()
        {
            InitializeComponent();
            /*treeViewEx1.AttachedSearchControl = enhancedTextBox1;*/




            //MessageBox.Show(DateTime.Now.AddYears(-10).Ticks.ToString());
            ////TabDragger DragTabs = new TabDragger(this.tabControl1);


            //byte[] publicKey = Assembly.LoadFile(@"C:\Program Files (x86)\SSMS Tools Pack\SSMS Tools Pack 2014\SSMSToolsPackC12.dll").GetName().GetPublicKey();
            //int length = (int)publicKey.Length - 12;
            //byte[] numArray = new byte[length];
            //Buffer.BlockCopy(publicKey, 12, numArray, 0, length);
            //using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
            //{
            //    rSACryptoServiceProvider.ImportCspBlob(numArray);

            //    SignedXml signedXml = new SignedXml();

            //    // Assign the key to the SignedXml object.
            //    signedXml.SigningKey = rSACryptoServiceProvider;

            //    // Create a reference to be signed.
            //    Reference reference = new Reference();

            //    // Add the passed URI to the reference object.
            //    reference.Uri = "";

            //    // Add the reference to the SignedXml object.
            //    signedXml.AddReference(reference);

            //    // Compute the signature.
            //    signedXml.ComputeSignature();

            //    // Get the XML representation of the signature and save 
            //    // it to an XmlElement object.
            //    XmlElement xmlDigitalSignature = signedXml.GetXml();

            //    // Save the signed XML document to a file specified 
            //    // using the passed string.
            //    XmlTextWriter xmltw = new XmlTextWriter("C:\\users\\adamp\\test.xml", new UTF8Encoding(false));
            //    xmlDigitalSignature.WriteTo(xmltw);
            //    xmltw.Close();
            //}
        }

        //private void Test1Task(int loopCount)
        //{
        //    textBox2.Text += "Testing TPL:" + Environment.NewLine + Environment.NewLine;
        //    var taskSyncList = new List<SyncTimes>();
        //    TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        //    DateTime taskStart = DateTime.Now;

        //    for (var i = 0; i < loopCount; i++)
        //    {
        //        var dt = new SyncTimes { Index = i, Started = DateTime.Now };
        //        Task.Factory.
        //            StartNew(s =>
        //            {
        //                return InLoopCall(s);
        //            }, dt).
        //            ContinueWith(t =>
        //            {
        //                taskSyncList = ReportCompletion(t.Result, taskSyncList, taskStart, loopCount);
        //                if (t.Result.Index == loopCount - 1 && loopCount == _loopCount)
        //                {
        //                    Test1BgWorker(loopCount);
        //                }
        //                else if (loopCount == 5)
        //                {
        //                    Test1BgWorker(5);
        //                }
        //            }, taskScheduler);
        //    }
        //}

        //private void Test1BgWorker(int loopCount)
        //{
        //    textBox2.Text += "Testing BG Worker:" + Environment.NewLine + Environment.NewLine;
        //    var taskSyncList = new List<SyncTimes>();

        //    DateTime taskStart = DateTime.Now;

        //    for (var i = 0; i < _loopCount; i++)
        //    {
        //        var dt = new SyncTimes { Started = DateTime.Now, Index = i };
        //        var bgWorker = new BackgroundWorker();

        //        bgWorker.DoWork +=
        //            (sender, args) =>
        //                args.Result = InLoopCall(args.Argument);

        //        bgWorker.RunWorkerCompleted +=
        //            (sender, args) =>
        //            {
        //                var param = (SyncTimes)args.Result;
        //                taskSyncList = ReportCompletion(param, taskSyncList, taskStart, loopCount);

        //                if (param.Index == loopCount - 1 && loopCount == _loopCount)
        //                {
        //                    Test1Task(5);
        //                }
        //            };

        //        bgWorker.RunWorkerAsync(dt);
        //    }
        //}

        //private void Test1Thread(int loopCount)
        //{
        //    textBox2.Text += "Testing Thread Object:" + Environment.NewLine + Environment.NewLine;
        //    var taskSyncList = new List<SyncTimes>();

        //    DateTime taskStart = DateTime.Now;

        //    for (var i = 0; i < _loopCount; i++)
        //    {
        //        var dt = new SyncTimes { Started = DateTime.Now, Index = i };

        //        var t = new Thread(Start);
        //        t.Start();
        //    }
        //}

        //private void Start(object o)
        //{

        //}

        private TreeNodeEx GetRandomNodes(TreeNodeEx parent, int level)
        {
            if (level > 10)
                return parent;

            parent.Text = GetRandomName();

            if (_randomNameNumber.Next(0, level) != 0)
                return parent;

            int r = _randomNameNumber.Next(0, 12);

            for (int i = 0; i < r; i++)
            {
                TreeNodeEx t = new TreeNodeEx(GetRandomName());
                var t2 = GetRandomNodes(t, level + 1);
                parent.Nodes.Add(t2);
            }

            return parent;
        }

        private string GetRandomName()
        {
            string firstName = _firstNames[_randomNameNumber.Next(_firstNames.Length - 1)];
            string lastName = _lastNames[_randomNameNumber.Next(_lastNames.Length - 1)];
            return firstName + " " + lastName;
        }

        private SyncTimes InLoopCall(object input)
        {
            var syncTime = (SyncTimes)input;
            syncTime.InTask = DateTime.Now;
            return syncTime;
        }

        //private List<SyncTimes> ReportCompletion(SyncTimes syncTime, List<SyncTimes> syncTimes, DateTime taskStart, int loopCount)
        //{
        //    syncTime.Ended = DateTime.Now;
        //    syncTimes.Add(syncTime);

        //    if (syncTime.Index == loopCount - 1)
        //    {
        //        var taskTotal = DateTime.Now - taskStart;

        //        double total = syncTimes.Sum(li => (li.Ended - li.Started).TotalMilliseconds);
        //        double avg = total / loopCount;

        //        double totalToInner = syncTimes.Sum(li => (li.InTask - li.Started).TotalMilliseconds);
        //        double avgToInner = totalToInner / loopCount;

        //        double totalToUi = syncTimes.Sum(li => (li.Ended - li.InTask).TotalMilliseconds);
        //        double avgToUi = totalToUi / loopCount;

        //        textBox2.Text += "Looped: " + loopCount + Environment.NewLine;
        //        textBox2.Text += "SyncTimes Count: " + syncTimes.Count + Environment.NewLine;
        //        textBox2.Text += "Actual total (ms): " + taskTotal.TotalMilliseconds.ToString("N2") + Environment.NewLine;
        //        textBox2.Text += "Accumulated total (ms): " + total.ToString("N2") + Environment.NewLine;
        //        textBox2.Text += "Avg to inner context (ms): " + avgToInner.ToString("N2") + Environment.NewLine;
        //        textBox2.Text += "Avg to UI (ms): " + avgToUi.ToString("N2") + Environment.NewLine;
        //        textBox2.Text += "Average (ms): " + avg.ToString("N2") + Environment.NewLine;
        //        textBox2.Text += "----------------------------------" + Environment.NewLine;
        //        Thread.Sleep(200);
        //    }

        //    return syncTimes;
        //}

        private void btnAddTestNodes_Click(object sender, EventArgs e)
        {
            treeViewEx1.Nodes.Clear();
            var random = new Random();
            int rootNodeCount = random.Next(14, 20);
            for (int i = 0; i < rootNodeCount; i++)
            {
                treeViewEx1.Nodes.Add(GetRandomNodes(new TreeNodeEx(), 1));
            }
        }

        //private void btnTest1_Click(object sender, EventArgs e)
        //{
        //    Test1Task(_loopCount);
        //    //BgWorkerTest1();
        //}

        private class SyncTimes
        {
            public int Index { get; set; }
            public DateTime Started { get; set; }
            public DateTime InTask { get; set; }
            public DateTime Ended { get; set; }
        }

        private void btnTest3_Click(object sender, EventArgs e)
        {
            treeViewEx1.ThemeStyle =
                treeViewEx1.ThemeStyle == TreeViewEx.TreeViewExThemeStyles.Default
                    ? TreeViewEx.TreeViewExThemeStyles.Explorer
                    : TreeViewEx.TreeViewExThemeStyles.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
