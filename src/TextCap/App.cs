using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TextCap
{
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {

            var loadCommandType = typeof(UpperCaseCommand);

            var textCapPullDownButtonData = new PulldownButtonData(loadCommandType.FullName, "Text Cap")

            {
                // Set the LargeImage of levelsButton using an embedded resource
                Image = new BitmapImage(new Uri("pack://application:,,,/TextCap;component/Resources/textcap-16.png", UriKind.RelativeOrAbsolute)),
                LargeImage = new BitmapImage(new Uri("pack://application:,,,/TextCap;component/Resources/textcap-32.png", UriKind.RelativeOrAbsolute))
            };

            var panel = application.CreateRibbonPanel("Text Cap");

            var pulldownButton=(PulldownButton)panel.AddItem(textCapPullDownButtonData);

            AddPushButton(pulldownButton, typeof(UpperCaseCommand), "Upper Case");
            AddPushButton(pulldownButton, typeof(LowerCaseCommand), "Lower Case");
            AddPushButton(pulldownButton, typeof(SentenceCaseCommand), "Sentence Case");
            AddPushButton(pulldownButton, typeof(TitleCaseCommand), "Title Case");

            return Result.Succeeded;
        }

        private static void AddPushButton(PulldownButton pullDownButton, Type command, string buttonText)
        {
            var buttonData = new PushButtonData(command.FullName, buttonText, Assembly.GetAssembly(command).Location, command.FullName);
            pullDownButton.AddPushButton(buttonData);
        }
    }
}
