
const Home = () => {
  Section.innerHTML = "";
};
const HomeImg = () => {
  Home();
  $dc.img(Section, "Imagenes/matriz.png").className = "Portada";
};


const Post = (Data) => {
  const req = new XMLHttpRequest();
  let res;
  const Change = () => {
    if (req.readyState === 4 && req.status === 200) res = req.responseText;
  };
  req.onreadystatechange = Change;

  req.open("POST", "Default.aspx", false);

  req.send(Data);
  return res;
};

const OnLoad = () => {
   
  try {
       
    Home();
    $n.init();
} catch (e) { alert(e);}

};
window.onload = OnLoad;
