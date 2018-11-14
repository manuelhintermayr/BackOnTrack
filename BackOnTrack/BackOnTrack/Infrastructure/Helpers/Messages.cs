using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackOnTrack.Infrastructure.Helpers
{
    public static class Messages
    {
        public static void CreateMessageBox(string message, string title, bool isError)
        {
                MessageBox.Show(message,
                    title,
                    MessageBoxButtons.OK,isError?
                    MessageBoxIcon.Error:MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }
    }
}
