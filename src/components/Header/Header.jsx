import styles from './Header.module.css';
import { NavLink } from 'react-router-dom';
import React, { useContext, useEffect, useState } from 'react';
import { StoreContext } from '../../store/StoreContext';

const Header = () => {
  const { state,dispatch } = useContext(StoreContext);
  const [isLogged, setIsLogged] = useState(false);

  useEffect(()=>{
    if (state.accessToken) setIsLogged(true);
  },[state.accessToken])
  console.log(state)
  return (
    <header className={styles.header}>
      <div className={styles.headerContainer}>
        <div className={styles.headerLogo}>AutoKonfigurator</div>
        <nav>
          <ul className={styles.navList}>
            <li className={styles.navItem}><NavLink to="/home">Modele</NavLink></li>
            <li className={styles.navItem}><NavLink to="/car-configurator">Konfigurator</NavLink></li>
            <li className={styles.navItem}>Salony</li>
            <li className={styles.navItem}>Finansowanie</li>
            <li className={styles.navItem}><NavLink to="/contact">Kontakt</NavLink></li>
            <li className={styles.navItem}><NavLink to="/login">Logowanie</NavLink></li>
          </ul>
        </nav>
      </div>
      <div className={styles.login}>
          {isLogged && (<p>Zalogowany</p>) }
      </div>
    </header>
  );
};

export default Header;
