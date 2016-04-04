using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace Project.WebUI.Helpers
{
    /// <summary>
    /// Checks browser version and gets their capabilities
    /// </summary>
    public class CustomerHttpCapabilitiesProvider : HttpCapabilitiesDefaultProvider
    {

        public override HttpBrowserCapabilities GetBrowserCapabilities(HttpRequest request)
        {

            HttpBrowserCapabilities browser = base.GetBrowserCapabilities(request);

            // Correct for IE 11, which presents itself as Mozilla version 0.0
            string ua = request.UserAgent;

            // Ensure IE by checking for Trident
            // Teports the real IE version, not the compatibility view version. 
            if (ua != null && ua.Contains(@"Trident"))
            {
                if (!browser.IsBrowser(@"IE"))
                {
                    browser.AddBrowser(@"ie");
                    browser.AddBrowser(@"ie6plus");
                    browser.AddBrowser(@"ie10plus");
                }

                IDictionary caps = browser.Capabilities;

                caps[@"Browser"] = @"IE";

                // Determine browser version
                bool ok = false;
                string majorVersion = null; // convertable to in
                string minorVersion = null; // convertable to double
                Match m = Regex.Match(ua, @"rv:(\d+)\.(\d+)");
                //(?:\b(MS)?IE\s+|\bTrident\/7\.0;.*\s+rv:)(\d+)
                if (m.Success)
                {
                    ok = true;
                    majorVersion = m.Groups[1].Value;
                    minorVersion = m.Groups[2].Value; // typically 0
                } else
                {
                    m = Regex.Match(ua, @"Trident/(\d+)\.(\d+)");
                    if (m.Success)
                    {
                        int v;
                        ok = int.TryParse(m.Groups[1].Value, out v);
                        if (ok)
                        {
                            v += 4; // Trident/7 = IE 11, Trident/6 = IE 10, Trident/5 = IE 9, and Trident/4 = IE 8
                            majorVersion = v.ToString(@"d");
                            minorVersion = m.Groups[2].Value; // typically 0
                        }
                    }
                }

                if (ok)
                {
                    caps[@"MajorVersion"] = majorVersion;
                    caps[@"MinorVersion"] = minorVersion;
                    caps[@"Version"] = String.Format(@"{0}.{1}", majorVersion, minorVersion);
                }
            }

            return browser;
        }
    }
}
