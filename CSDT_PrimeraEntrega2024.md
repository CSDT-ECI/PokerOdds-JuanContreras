## Primera entrega 📦

_Para esta entrega he decidido integrar [Sonarqube](https://docs.sonarsource.com/sonarqube/10.3/) con [Github Actions](https://docs.github.com/es/actions) ejecutando Sonarqube en local y exponiéndolo al exterior con [ngrok](https://ngrok.com/), para analizar el codigo del proyecto_

## Pasito a pasito 🛠️

_En el apartao de Git Actions cree un workflow que al hacer un merge hacia la rama master se ejecutara y como tarea iba a compilar el proyecto y ejecutar las pruebas, pero este por la version d e .net aunque la ajustara estaba dando error_
![Texto alternativo](/images/cat1.png)(width=50%)

_Cree un workflow que me permitia realizar una integración de sonarqube_

~~~yml
name: SonarQube analysis
on:
  push:
    branches: [ "master" ]
  workflow_dispatch:
jobs:
  Analysis:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0 
      - name: Analyze with SonarQube
        uses: SonarSource/sonarqube-scan-action@v2.0.1
        env:
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}   
          SONAR_HOST_URL: ${{ vars.SONAR_HOST_URL }}
~~~
_Despues de realizar la configuracion de tokens, secretos y variables correspondientes y de levantar en doker ngrok y sonarQube podia acceder desde la url expuesta, para que despues e la ejecucion del woekflow pueda ver los resultados aqui_
![Lint](/images/Qube1.png)

_Adicionalmente descargue el complemento de SonarLint para VisualStudio y lo conecte con el dominio generado en ngrok  como se aprecia en la imagena  continuacion_
![Lint](/images/Lint.png)

## Análisis  📖
![análisis](/images/piensa.png)(width=50%)
# SonarQube
SonarQube es una herramienta invaluable para la gestión de la deuda técnica. Su capacidad para identificar, medir, priorizar y reducir la deuda técnica  es evidente una vez se entra  revisar el resultado del analisis de un proyecto, aun más cuando es un caso como este en el que hemos venido revisando la deuda tecnica de manera manual.

Lo primero en llamar mi atencion es como el proyecto a pesar de contar ya con un pequeño grupo de pruebas unitarias y de que por mi parte agregue unas cuantas mas, en Sonar se ve reflejado un 0% de coverage, posiblemente se deba a algun tema de configuración que por el momento no logre solucionar.

Juegando un poco en el sonar con las Quality Gates , cree una nueva con exigencias similares a las  trabajadas en un entorno mas real segun mi exoeriencia y dispare nuevamente el workflow esperando que fallara el quality gate,  copie la opcion que me daba el sonar  para que en el readme se lograra ver una etiqueta que muestra si el  paso o no paso, adjunto la logica a continuación  para los archivos md , pero  cuando se revise el trabajo no se podra ver adecuadamente ya que su funcionamiento depende de la ejecucion de sonar y ngrok en doker dada la naturaleza del desarrollo propuesto para esta actividad.

[![Quality gate](https://bluejay-ethical-strangely.ngrok-free.app/api/project_badges/quality_gate?project=hello-sonar-ngrok&token=sqb_c75dbed51b0f804e2d1a16e2752c291ce2d86a48)](https://bluejay-ethical-strangely.ngrok-free.app/dashboard?id=hello-sonar-ngrok)

En el apartado de Issues, gracias al filtrado podremos tener claras las prioridades gracias a la severidad con la que nos marca cada item ,  tambien nos dice de que tipo son (bug, code smell o vulnerabilidad)
![Lint](/images/Qube2.png)

Finalmente al ingresar a ver en detalle el Issue a trabajar, tenderemos un apartado fantastico para el desarrollador, donde muestra con exactitud donde esta el problema , porque eso es un problema, como se podria solucionar, entre otras pestañas que facilitan en gran medida esta tediosa pere necesaria labor
![Lint](/images/Qube3.png)
