// EmailJS service jako alternatywa dla API
// Wymaga zainstalowania: npm install @emailjs/browser

export const sendEmailViaEmailJS = async (formData) => {
  // To jest przykład implementacji EmailJS
  // Wymaga konfiguracji konta na emailjs.com
  
  const emailJSConfig = {
    serviceId: 'service_konfigurator', // Pobierz z EmailJS
    templateId: 'template_kontakt',    // Pobierz z EmailJS  
    publicKey: 'YOUR_PUBLIC_KEY'       // Pobierz z EmailJS
  };

  try {
    // Importuj EmailJS dynamicznie
    const emailjs = await import('@emailjs/browser');
    
    const templateParams = {
      from_name: formData.name,
      from_email: formData.email,
      phone: formData.phone,
      subject: getSubjectText(formData.subject),
      message: formData.message,
      to_email: 'konfiguratorsamochodwy@gmail.com'
    };

    const response = await emailjs.send(
      emailJSConfig.serviceId,
      emailJSConfig.templateId,
      templateParams,
      emailJSConfig.publicKey
    );

    console.log('Email sent via EmailJS:', response);
    return { success: true };
  } catch (error) {
    console.error('EmailJS error:', error);
    return { success: false, error: error.message };
  }
};

const getSubjectText = (subject) => {
  const subjects = {
    'info': 'Informacje ogólne',
    'config': 'Konfiguracja samochodu', 
    'test': 'Jazda testowa',
    'other': 'Inne zapytanie'
  };
  return subjects[subject] || 'Nowe zapytanie';
};

// Instrukcja użycia:
// 1. npm install @emailjs/browser
// 2. Załóż konto na emailjs.com
// 3. Skonfiguruj szablon email
// 4. Zastąp konfigurację powyżej
// 5. Użyj w komponencie Contact.jsx jako fallback