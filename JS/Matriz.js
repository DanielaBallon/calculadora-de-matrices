// Función para crear la interfaz de usuario

const $$Matriz = function (){

 this.crearInterfazUsuario=()=> {
    // Crear un contenedor principal
    const container = $dc.div(Section);
    container.id = "container";
  
    // Crear un título
    $dc.h1(container, "Calculadora de Matrices");
  
    // Crear sección para la primera matriz
    const matrizASection = $dc.div(container);
    matrizASection.id = "matrizASection";
    $dc.h2(matrizASection, "Matriz A");
  
    // Crear sección para la segunda matriz
    const matrizBSection = $dc.div(container);
    matrizBSection.id = "matrizBSection";
    $dc.h2(matrizBSection, "Matriz B");
  
    // Crear select para elegir la operación
    const operacionSelect = $dc.addSelect(container, "Operación");
    $dc.Option(operacionSelect, "Sumar", "sumar");
    $dc.Option(operacionSelect, "Restar", "restar");
    $dc.Option(operacionSelect, "Multiplicar", "multiplicar");

    // Crear botón para realizar la operación
    const operarButton = $dc.addInput(container, "button");
    operarButton.value = "Realizar Operación";
    operarButton.onclick = () => this.realizarOperacion(operacionSelect.value);
  
    // Crear sección para mostrar el resultado
    const resultadoSection = $dc.div(container);
    resultadoSection.id = "resultadoSection";
    $dc.h2(resultadoSection, "Resultado de la Operación");
  
    // Añadir elementos de entrada para la Matriz A
    this.crearInputsMatriz(matrizASection, "A");
  
    // Añadir elementos de entrada para la Matriz B
    this.crearInputsMatriz(matrizBSection, "B");
  }
  
  // Función para crear los inputs de una matriz
 this.crearInputsMatriz=(section, nombreMatriz)=> {
 
    const filas = parseInt(prompt(`Ingrese el número de filas para la Matriz ${nombreMatriz} :`));
    const columnas = parseInt(prompt(`Ingrese el número de columnas para la Matriz ${nombreMatriz} :`));
  
    const tabla = $d.ce("table");
    section.appendChild(tabla);
  
    for (let i = 0; i < filas; i++) {
      const fila = $d.ce("tr");
      tabla.appendChild(fila);
  
      for (let j = 0; j < columnas; j++) {
        const celda = $d.ce("td");
        fila.appendChild(celda);
  
        const input = $dc.addInput(celda, "text");
        input.placeholder = `${nombreMatriz}[${i + 1}][${j + 1}]`;
      }
    }
  }
  

this.realizarOperacion = (operacion) => {
    const matrizA = this.obtenerMatrizDesdeInputs("A");
    const matrizB = this.obtenerMatrizDesdeInputs("B");
    // Verificar si la operación es válida
 
  if (!esOperacionValida(matrizA, matrizB, operacion)) {
    alert("No se puede realizar la operación. Verifica las dimensiones de las matrices.");

    
    return;}

    let resultado;

    switch (operacion) {
      case "sumar":
        resultado = this.sumarMatrices(matrizA, matrizB);
        break;
      case "restar":
        resultado = this.restarMatrices(matrizA, matrizB);
        break;
      case "multiplicar":
        resultado = this.multiplicarMatrices(matrizA, matrizB);
        break;
      default:
        console.error("Operación no reconocida");
        return;
    }
    this.mostrarResultado(resultado);
}
  // Función para obtener una matriz desde los inputs en la interfaz
this.obtenerMatrizDesdeInputs = (nombreMatriz) => {
    const matriz = [];
  
    const tabla = $d.id(`matriz${nombreMatriz}Section`).getElementsByTagName("table")[0];
    const filas = tabla.rows;
  
    for (let i = 0; i < filas.length; i++) {
      const fila = filas[i];
      const celdas = fila.cells;
  
      const filaMatriz = [];
  
      for (let j = 0; j < celdas.length; j++) {
        const input = celdas[j].getElementsByTagName("input")[0];
        const valor = input.value.trim();  // Obtener el valor y quitar espacios en blanco al principio y al final
        
        if (!valor || isNaN(valor)) {
          // Valor no es numérico o es vacío
          alert(`Ingresa solo valores numéricos en la matriz ${nombreMatriz}.`);
          return;
        }
  
        filaMatriz.push(parseFloat(valor));
      }
  
      matriz.push(filaMatriz);
    }
  
    return matriz;
  };
  

  
  // Función para verificar si una operación es válida
  const esOperacionValida = (matrizA, matrizB, operacion) => {
    const filasA = matrizA.length;
    const columnasA = matrizA[0].length;
    const filasB = matrizB.length;
    const columnasB = matrizB[0].length;
  
  
    switch (operacion) {
      case 'sumar':
      case 'restar':
        // Verificar que las matrices tengan la misma dimensión
        return filasA === filasB && columnasA === columnasB;
  
      case 'multiplicar':
        // Verificar que el número de columnas de A sea igual al número de filas de B
        return columnasA === filasB;
  
      default:
        return false;
    }
  };
  

  
  // Función para sumar dos matrices
this. sumarMatrices=(matrizA, matrizB)=> {
    const resultado = [];
  
    for (let i = 0; i < matrizA.length; i++) {
      const filaResultado = [];
  
      for (let j = 0; j < matrizA[i].length; j++) {
        filaResultado.push(matrizA[i][j] + matrizB[i][j]);
      }
  
      resultado.push(filaResultado);
    }
  
    return resultado;
  }
this.restarMatrices = (matrizA, matrizB) => {
    const resultado = [];

    for (let i = 0; i < matrizA.length; i++) {
      const filaResultado = [];

      for (let j = 0; j < matrizA[i].length; j++) {
        filaResultado.push(matrizA[i][j] - matrizB[i][j]);
      }

      resultado.push(filaResultado);
    }

    return resultado;
  };

// Función para multiplicar dos matrices
this.multiplicarMatrices = (matrizA, matrizB) => {
    const filasA = matrizA.length;
    const columnasA = matrizA[0].length;
    const filasB = matrizB.length;
    const columnasB = matrizB[0].length;
  
      
    const resultado = [];
  
    for (let i = 0; i < filasA; i++) {
      const filaResultado = [];
  
      for (let j = 0; j < columnasB; j++) {
        let suma = 0;
  
        for (let k = 0; k < columnasA; k++) {
          suma += matrizA[i][k] * matrizB[k][j];
        }
  
        filaResultado.push(suma);
      }
  
      resultado.push(filaResultado);
    }
  
    return resultado;
  };
  
  
  // Función para mostrar el resultado en la interfaz
 this.mostrarResultado=(matriz)=> {
    const resultadoSection = $d.id("resultadoSection");
  
    // Limpiar contenido anterior
    resultadoSection.innerHTML = "";
  
    // Crear tabla para mostrar la matriz resultado
    const tabla = $d.ce("table");
    resultadoSection.appendChild(tabla);
  
    for (let i = 0; i < matriz.length; i++) {
      const fila = $d.ce("tr");
      tabla.appendChild(fila);
  
      for (let j = 0; j < matriz[i].length; j++) {
        const celda = $d.ce("td");
        fila.appendChild(celda);
  
        const input = $dc.addInput(celda, "text");
        input.value = matriz[i][j];
        input.disabled = true;
      }
    }
  }
  
}

//verificar que los campos sean validos

const $m = new $$Matriz();
