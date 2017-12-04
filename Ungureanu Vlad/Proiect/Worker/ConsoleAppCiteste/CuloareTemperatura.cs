using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleAppCiteste
{

    /* -20 -> +50*/
    public enum CuloareTemperatura
    {
            Purple = 50,/*#800080*/ /*50-49*/
        Purple2=48,/*#990066*/ /*48-47*/
        Purple3=46,/*#B3004D*/ /*46-45*/
        Purple4=44,/*#CC0033*/ /*44-43*/
        Purple5=42,/*#E6001A*/ /*42-41*/
             Red = 40,/*#FF0000*/  /*40*/
        Red2=38,/*#FF3700*/ /*39-38*/
        Red3=36,/*#FF6E00*/ /*37-36*/
        Orange = 35,/*#FFA500*/ /*35*/
        Orange2 = 34,/*#FFC300*/ /*34-33*/
        Orange3 = 32,/*#FFE100*/ /*32-31*/
            Yellow = 30,/*#FFFF00*/ /*30*/
        Yellow2 = 28,/*#AAFF00*/ /*29-28*/
        Yellow3 = 26,/*#55FF00*/ /*27-26*/
        Lime = 25,/*#00FF00*/ /*25*/
        Lime2 = 24,/*#00D500*/ /*24-23*/
        Lime3 = 22,/*#00AA00*/ /*22-21*/
             Green = 20,/*#008000*/ /*20*/
        Green2 = 18,/*#15A045*/ /*19-18*/
        Green3 = 16,/*#2BC08B*/ /*17-16*/
        Turquoise = 15,/*#40E0D0*/ /*15*/
        Turquoise2 = 14,/*#58DAD9*/ /*14-13*/
        Turquoise3 = 12,/*#6FD4E2*/ /*12-11*/
            SkyBlue = 10,/*#87CEEB*/ /*10*/
        SkyBlue2 = 8,/*#7BBBEC*/ /*9-8*/
        SkyBlue3 = 6,/*#70A8EC*/ /*7-6*/
        CornflowerBlue = 5,/*#6495ED*/ /*5*/
        CornflowerBlue2 = 4,/*#4D93F3*/ /*4-3*/
        CornflowerBlue3 = 2,/*#3592F9*/ /*2-1*/
            DodgerBlue = 0,/*#1E90FF*/ /*0*/
        DodgerBlue2 = -2,/*#2A83F5*/ /*-(1-2)*/
        DodgerBlue3 = -4,/*#3576EB*/ /*-(3-4)*/
        RoyalBlue = -5,/*#4169E1*/ /*-5*/
        RoyalBlue2 = -6,/*#2B46C1*/ /*-(6-7)*/
        RoyalBlue3 = -8,/*#1623A0*/ /*-(8-9)*/
            Navy = -10,/*#000080*/ /*-10*/
        Navy2 = -12,/*#00006B*/ /*-(11-12)*/
        Navy3 = -14,/*#000055*/ /*-(13-14)*/
        NavyBlack = -15,/*#000040*/ /*-15*/
        NavyBlack2 = -16,/*#00002B*/ /*-(16-17)*/
        NavyBlack3 = -18,/*#000015*/ /*-(18-19)*/
            Black = -20/*#000000*/ /*-20*/
    }

    public class Culoare
    {
        public CuloareTemperatura culoareTemperatura;
        public int valoare;
    }
}
