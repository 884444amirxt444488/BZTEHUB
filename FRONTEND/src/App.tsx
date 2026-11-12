import { useState } from 'react'
import './App.css'
import Footer from './Footer'
import Header from './Header'
import Image2 from "./assets/images.jpg"
import { Search } from 'lucide-react'

function App() {

  const [theme, setTheme] = useState("light");



  return (
    <html lang='fa' dir='rtl' data-theme={theme}>
      <Header theme={theme} setTheme={setTheme} />

      <div className='total_body'>
        <div className='total_banner_image'>
          <img src={Image2} alt='none' className='banner_image' />
        </div>
        <div className='banner_in'>
          <div className="banner_in2">
            <Search />
            <input type='text' placeholder='جست و جو' className='ba_in' />
          </div>
        </div>


      </div>



      <Footer />
    </html>
  )

}

export default App
