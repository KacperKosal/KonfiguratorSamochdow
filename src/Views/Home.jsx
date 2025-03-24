import CarConfigurator from "../components/CarConfigurator/CarConfigurator";
import CarViewer from "../components/CarConfigurator/CarViewer";
import Footer from "../components/Footer";
import Header from "../components/Header";

export default function Home(){
    return<>
    <Header/>
    <CarConfigurator/>
    <CarViewer/>
    <Footer/>
    </>
}