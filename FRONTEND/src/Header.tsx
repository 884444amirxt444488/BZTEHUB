
import { Moon, Sun, Menu } from "lucide-react"

import Image from "./assets/Logo2.png"



type Props = {
    theme: string,
    setTheme: React.Dispatch<React.SetStateAction<string>>
}


export default function Header({theme, setTheme}: Props) {

    const ChangeMode = () => {
        setTheme(theme === "light" ? "dark" : "light")
    }

    return (
        <div className="total_header" dir="ltr">
            <div className="header_image">
                <img src={Image} alt="Logo" className="logo_image" />
            </div>
            <div>
                <button className="header_menu_btn" onClick={ChangeMode}>
                    {
                        theme === "light"
                        ? <Moon />
                        : <Sun />
                    }
                </button>
            </div>
            <div>
                <button className="header_menu_btn">
                    <Menu />
                </button>
            </div>





        </div>
    )
}



