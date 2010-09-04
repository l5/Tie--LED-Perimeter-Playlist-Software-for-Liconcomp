using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tie.Controllers
{
    public abstract class BaseController
    {
        private App _app = null;

        public BaseController(App app)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            this._app = app;
        }

        public App Application
        {
            get
            {
                return this._app;
            }
        }
    }
}
