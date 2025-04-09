import React from 'react';
import styles from './Banner.module.css';

const Banner = () => {
  return (
    <div className={styles.banner}>
      <div className={styles.container}>
        <h1 className={styles.title}>Odkryj nasze modele</h1>
        <p className={styles.description}>
          Wybierz samochód dopasowany do Twoich potrzeb i skonfiguruj go według własnych preferencji.
        </p>
      </div>
    </div>
  );
};

export default Banner;
