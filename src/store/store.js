export const initialState = {
  accessToken: localStorage.getItem("accessToken")
};

export function reducer(state, action) {
    console.log(action.payload)
  switch (action.type) {
    case 'SET_TOKEN':
        localStorage.setItem('accessToken', action.payload)
      return { ...state, accessToken: action.payload };
    case 'LOGOUT':
        localStorage.removeItem('accessToken')
      return { ...state, accessToken: null };
    default:
      return state;
  }
}
