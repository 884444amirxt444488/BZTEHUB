
import { GraduationCap } from "lucide-react"
import { SiGithub, SiTelegram, SiGmail  } from "react-icons/si";




export default function Footer() {
    return (
        <div className="totla_footer" dir="rtl">
            <div className="total_footer_s_header">
                <div className="footer_s_header">
                    <GraduationCap size={40} />
                    <h2>BZTEHUB</h2>
                </div>
                <div className="identity_icons">
                    <SiGithub className="identity_icon" style={{
                        color: "black"
                    }} size={40} />
                    <SiTelegram style={{
                        color: "blue"
                    }} className="identity_icon" size={40} />
                    <SiGmail style={{
                        color: "red"
                    }} className="identity_icon" size={40} />
                </div>
            </div>
            <h4 className="footer_t">سامانه جامع اطلاعات دانشگاهی؛ دسترسی آسان‌تر به اطلاعات و منابع آموزشی دانشگاه</h4>
            <hr />
            <div className="total_explains">
                <div className="right_e">
                    <h3>دسترسی سریع</h3>
                    <p className="footer_s_p">استادان</p>
                    <p className="footer_s_p">رشته ها</p>
                    <p className="footer_s_p">مکان ها</p>
                    <p className="footer_s_p">منابع و جزوات</p>
                    <p className="footer_s_p">برنامه درسی</p>
                </div>
                <div className="left_e">
                    <h3>راه های ارتباطی با ما</h3>
                    <p className="footer_s_p">اینیستاگرام</p>
                    <p className="footer_s_p">جیمیل</p>
                    <p className="footer_s_p">تلگرام</p>
                    <p className="footer_s_p">واتس اپ</p>
                    <p className="footer_s_p">توییتر</p>
                </div>
            </div>
            <hr />
            <h4>© 2026 BZTEHUB. All rights reserved</h4>



        </div>
    )
}







