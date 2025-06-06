import React from 'react';
import axiosInstance from '../../services/axiosConfig';

const TokenTestButton = () => {
  const testDirectRefresh = async () => {
    console.log('=== TESTING DIRECT REFRESH ===');
    try {
      console.log('Attempting direct refresh call...');
      const response = await axiosInstance.post('/refresh-jwt');
      console.log('Direct refresh successful:', response.data);
    } catch (error) {
      console.error('Direct refresh failed:', error);
    }
  };

  return (
    <div style={{ position: 'fixed', top: '10px', right: '10px', zIndex: 9999, display: 'flex', flexDirection: 'column' }}>
      <button 
        onClick={testDirectRefresh}
        style={{ 
          padding: '8px', 
          margin: '2px', 
          backgroundColor: '#28a745', 
          color: 'white', 
          border: 'none',
          cursor: 'pointer',
          fontSize: '12px'
        }}
      >
        Direct Refresh
      </button>
    </div>
  );
};

export default TokenTestButton;