using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static List<HtmlElement> GetElementsByClass(string className, string ancestorId = null) {
            var ancestor = Document.Body;
            if (ancestorId != null)
                ancestor = GetElementById(ancestorId);
            return getOffspringByClass(ancestor, className, false);
        }

        public static List<HtmlElement> getOffspringByClass(HtmlElement element, string className) {
            return getOffspringByClass(element, className, false);
        }

        public static List<HtmlElement> GetChildrenByClass(HtmlElement element, string className) {
            return getOffspringByClass(element, className, true);
        }

        public static List<HtmlElement> GetSiblingsByClass(HtmlElement element, string className) {
            return getOffspringByClass(element.Parent, className, true);
        }

        private static List<HtmlElement> getOffspringByClass(HtmlElement parent, string className, bool childrenOnly) {
            HtmlElementCollection elements = parent.All;
            if (childrenOnly)
                elements = parent.Children;

            List<HtmlElement> result = new List<HtmlElement>();
            foreach (HtmlElement element in elements) {
                if (element.GetAttribute("className").Split(' ').Contains(className))
                    result.Add(element);
            }
            return result;
        }

        public static List<HtmlElement> GetElementsByIdLike(string idRegEx, string ancestorId = null) {
            var ancestor = Document.Body;
            if (ancestorId != null)
                ancestor = GetElementById(ancestorId);
            var elements = ancestor.All;

            List<HtmlElement> result = new List<HtmlElement>();
            foreach (HtmlElement element in elements) {
                if (element.Id != null && Regex.IsMatch(element.Id, idRegEx))
                    result.Add(element);
            }
            return result;
        }




        public static string GetClassName(HtmlElement element) {
            return element.GetAttribute("className");
        }

        public static string GetClassName(string elementId) {
            return GetClassName(Document.GetElementById(elementId));
        }




        public static bool isVisible(HtmlElement element) {
            for (var el = element; true; el = el.Parent) {
                if (el == null)
                    throw new NullReferenceException("Element's visibility cannot be evaluated, one of his ancestors is null");
                if (el.TagName.ToLower() == "body")
                    return true;
                if (isHidden(el))
                    return false;
            }
        }

        public static bool isAnyVisible(List<HtmlElement> elements) {
            foreach (HtmlElement element in elements) {
                if (isVisible(element))
                    return true;
            }
            return false;
        }

        public static bool isVisible(string elementId) {
            return isVisible(Document.GetElementById(elementId));
        }

        public static bool isHidden(HtmlElement element) {
            return element.Style != null && element.Style.Contains("display: none");
        }

        public static bool isHidden(string elementId) {
            return isHidden(Document.GetElementById(elementId));
        }




        public static void Click(HtmlElement element) {
            element.InvokeMember("click");
        }

        public static void Click(string elementId, string offspringClassName = null) {
            InvokeMember("click", elementId, offspringClassName);
        }
        
        public static bool TryClick(string elementId, string offspringClassName = null) {
            try {
                Click(elementId, offspringClassName);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static void InvokeMember(string member, string elementId, string offspringClassName = null) {
            var element = Document.GetElementById(elementId);
            if (offspringClassName != null)
                element = getOffspringByClass(element, offspringClassName)[0];
            if (element == null)
                throw new NullReferenceException("Tried to invoke '" + member + "' on element with id '" + elementId + "' but it doesn't exist");
            element.InvokeMember(member);
        }

        public static void InvokeScript(string functionName) {
            Document.InvokeScript(functionName);
        }




        public static void WaitForId(string id, int postWaitTime = 2000, int timeout = 60000) {
            if (!TryWaitForId(id, timeout, postWaitTime))
                throw new Exception("Wait for ID Timeout (#" + id + ")");
        }

        public static bool TryWaitForId(string id, int timeout = 60000, int postWaitTime = 2000) {
            Func<bool> predicate = () => {
                var element = Document.GetElementById(id);
                return element != null && isVisible(element);
            };
            return WaitFor(predicate, postWaitTime, timeout);
        }

        public static void WaitForClass(string className, string ancestorId = null, int waitTime = 2000) {
            Func<bool> predicate = () => {
                var elements = GetElementsByClass(className, ancestorId);
                return elements.Any() && isAnyVisible(elements);
            };
            if (!WaitFor(predicate, waitTime)) {
                string selector = "." + className;
                if (ancestorId != null)
                    selector = "#" + ancestorId + " " + selector;

                throw new Exception("Wait for class Timeout (" + selector + ")");
            }
        }

        public static void WaitForBus() {
            Wait(1000);
            Func<bool> predicate = () => {
                var busBox = GetElementById("travel_box");
                return busBox == null || !isVisible(busBox) || !busBox.GetAttribute("className").Any();
            };
            if (!WaitFor(predicate, 0))
                throw new Exception("Wait for bus Timeout");
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

        public static bool WaitFor(Func<bool> predicate, long postWaitTime = 1000, long timeout = 60000) {
            var start = DateTime.Now;
            while (true) {
                if (predicate()) {
                    Wait(postWaitTime);
                    return true;
                }

                if ((DateTime.Now - start).TotalMilliseconds > timeout)
                    return false;

                Application.DoEvents();
                Thread.Sleep(10);
            }
        }



        public static void RemoveElementById(string elementId) {
            var ad = Document.GetElementById(elementId);
            if (ad != null)
                ad.OuterHtml = "";
        }
        
        public static void SetPermanentStyle(string elementId, string style) {
            string stylesId = "wf_manager_styles";
            var styles = Document.GetElementById(stylesId);
            if (styles == null) {
                styles = Document.CreateElement("style");
                styles.Id = stylesId;
                Document.Body.AppendChild(styles);
            }
            styles.InnerHtml += "#" + elementId + " { " + style + " !important; } ";
        }



        public static void MoveCursorToElement(string elementId) {
            MoveCursorToElement(Document.GetElementById(elementId));
        }

        public static void MoveCursorToElement(HtmlElement element) {
            IHTMLRect rect = ((IHTMLElement2)element.DomElement).getBoundingClientRect();
            Size size = element.ClientRectangle.Size;
            Point point = b.Parent.PointToScreen(b.Location);
            point.Offset(rect.left, rect.top);
            point.Offset(size.Width / 2, size.Height / 2);
            Cursor.Position = point;
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
