using System.Windows.Forms;

namespace BackOnTrack.Infrastructure.Helpers
{
    public static class Messages
    {
        public static void CreateMessageBox(string message, string title, bool isError)
        {
                MessageBox.Show($"BackOnTrack - {message}",
                    title,
                    MessageBoxButtons.OK,isError?
                    MessageBoxIcon.Error:MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
        }
    }
}
