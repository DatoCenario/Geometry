FROM microsoft/dotnet

RUN echo "Write folder where application should be deployed."

RUN read DEPLOYDIR

RUN mkdir $DEPLOYDIR

RUN cd $DEPLOYDIR

