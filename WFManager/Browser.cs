using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFManager {
    public static class Browser {

        public static WebBrowser b;



        public static HtmlDocument Document {
            get {
                return b.Document;
            }
        }



        public static void Initialize(WebBrowser webBrowser) {
            b = webBrowser;
        }



        public static HtmlElement GetElementById(string id) {
            return Document.GetElementById(id);
        }

        public static List<HtmlElement> GetElementsByClass(string className, string tagName = null) {
            return getByClass(Document.Body, className, tagName, false);
        }

        public static List<HtmlElement> getOffspringByClass(HtmlElement element, string className, string tagName = null) {
            return getByClass(element, className, tagName, false);
        }

        public static List<HtmlElement> GetChildrenByClass(HtmlElement element, string className, string tagName = null) {
            return getByClass(element, className, tagName, true);
        }

        public static List<HtmlElement> GetSiblingsByClass(HtmlElement element, string className, string tagName = null) {
            return getByClass(element.Parent, className, tagName, true);
        }

        private static List<HtmlElement> getByClass(HtmlElement parent, string className, string tagName, bool childrenOnly) {
            HtmlElementCollection elements = parent.All;
            if (childrenOnly)
                elements = parent.Children;
            if (tagName != null)
                elements.GetElementsByName(tagName);

            List<HtmlElement> result = new List<HtmlElement>();
            foreach (HtmlElement element in elements) {
                if (element.GetAttribute("className").Split(' ').Contains(className))
                    result.Add(element);
            }
            return result;
        }



        private static bool isVisible(HtmlElement element) {
            for (var el = element; true; el = el.Parent) {
                if (el == null)
                    throw new NullReferenceException("Element's visibility cannot be evaluated, one of his ancestors is null");
                if (el.TagName.ToLower() == "body")
                    return true;
                if (el.Style != null && el.Style.Contains("display: none"))
                    return false;
            }
        }

        private static bool isAnyVisible(List<HtmlElement> elements) {
            foreach (HtmlElement element in elements) {
                if (isVisible(element))
                    return true;
            }
            return false;
        }



        public static void Click(string elementId, string className = null) {
            var element = Document.GetElementById(elementId);
            if (className != null)
                element = getOffspringByClass(element, className)[0];
            if (element == null)
                throw new NullReferenceException("Tried to click element with id " + elementId + " but it doesn't exist");
            element.InvokeMember("click");
        }

        public static bool TryClick(string elementId, string className = null) {
            try {
                Click(elementId, className);
                return true;
            } catch (Exception) {
                return false;
            }
        }



        public static void WaitForId(string id, int postWaitTime = 2000, int timeout = 60000) {
            var start = DateTime.Now;
            while (true) {
                var element = Document.GetElementById(id);
                if (element != null && isVisible(element))
                    break;

                if ((DateTime.Now - start).TotalMilliseconds > timeout)
                    throw new Exception("Wait for ID Timeout");

                Application.DoEvents();
                Thread.Sleep(10);
            }

            Wait(postWaitTime);
        }

        public static bool TryWaitForId(string id, int timeout = 60000, int postWaitTime = 2000) {
            try {
                WaitForId(id, postWaitTime, timeout);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static void WaitForClass(string className, string tagName = null, int waitTime = 2000) {
            var start = DateTime.Now;
            while (true) { 
                var elements = GetElementsByClass(className, tagName);
                if (elements.Any() && isAnyVisible(elements))
                    break;

                if ((DateTime.Now - start).TotalMilliseconds > 60000)
                    throw new Exception("Wait for class Timeout");

                Application.DoEvents();
                Thread.Sleep(10);
            }

            Wait(waitTime);
        }

        public static void WaitForBus() {
            var start = DateTime.Now;
            Wait(500);
            while (true) {
                HtmlElement busBox = GetElementById("travel_box");
                if (busBox == null || !isVisible(busBox) || !busBox.GetAttribute("className").Any())
                    break;

                if ((DateTime.Now - start).TotalMilliseconds > 60000)
                    throw new Exception("Wait for bus Timeout");

                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        public static void WaitForDocument() {
            while (b.IsBusy) {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            var start = DateTime.Now;
            while (b.ReadyState != WebBrowserReadyState.Complete) {
                if ((DateTime.Now - start).TotalMilliseconds > 60000)
                    throw new Exception("Wait for document Timeout");

                Application.DoEvents();
                Thread.Sleep(10);
            }
            Application.DoEvents();
        }

        public static void Wait(long waitTime) {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < waitTime) {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }



        public static void SetValue(string elementId, string value) {
            Document.GetElementById(elementId).SetAttribute("Value", value);
        }



        public static void Navigate(string url) {
            b.Navigate(url);
            WaitForDocument();
        }



        public static void Lock() {
            Monitor.Enter(b);
        }

        public static void Release() {
            Monitor.Exit(b);
        }
    }
}
