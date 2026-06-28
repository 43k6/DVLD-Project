using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classes
{
    public class clsUtil
    {
        public static string GenerateGUID()
        {

            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();

            // convert the GUID to a string
            return newGuid.ToString();

        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {

            // Check if the folder exists
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    // If it doesn't exist, create the folder
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;

        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            // Full file name. Change your file name   
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;

        }

        public static bool CopyImageToProjectImagesFolder2(ref string sourceFile)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

            string DestinationFolder = @"C:\DVLD-People-Images\";
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);
            try
            {
                File.Copy(sourceFile, destinationFile, true);

            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            // 1. الحصول على مسار مجلد التشغيل الحقيقي (عادة يكون bin\Debug داخل مجلد الـ WinForms)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 2. الصعود للأعلى للوصول إلى المجلد الرئيسي للمشروع، ثم الدخول إلى مجلد الصور المستهدف
            // نستخدم Path.GetFullPath لتنظيف المسار والتخلص من علامات الـ ".." الزائدة
            string DestinationFolder = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\DVLD-People-Images"));

            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            // 3. دمج المسار الجديد مع اسم الملف الجديد المستبدل بـ GUID
            string destinationFile = Path.Combine(DestinationFolder, ReplaceFileNameWithGUID(sourceFile));

            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }
    }
}
