namespace Hotfix
{
    enum Toast_Gravity
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        CENTER
    }

    public enum Toast_Type
    {
        TEXT,
        COLOR_TEXT_IMAGE
    }
    
    public class Toast
    {
        public static Toast makeText(string name,Toast_Type type,float showTime = 1)
        {
            Toast toast = new Toast(name,type,showTime);

            return toast;
        }


        public Toast(string name,Toast_Type type,float showTime)
        {
            
        }

        public Toast Show()
        {

            return this;
        }


        public Toast SetGravity()
        {

            return this;
        }





    }
}