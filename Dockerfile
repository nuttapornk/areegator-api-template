FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["." , "."]
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --property:PublishDir=/app/publish

FROM base AS final
ENV DEBIAN_FRONTEND=noninteractive
ARG USERNAME=insuser
ARG USER_UID=1000
ARG USER_GID=$USER_UID
ARG BASE_PATH=/home/insuser/workspaces
ARG APP_STARTPOINT=_NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.dll

COPY .script-build/custom-scripts/* .script-build/library-scripts/* /tmp/staging/
# This path should be move to shell script
RUN apt-get update \
    && apt install zlib1g
    #
    # Create a group and user
RUN addgroup --gid ${USER_GID} ${USERNAME} \
    && adduser --gid ${USER_GID} --uid ${USER_UID} ${USERNAME}
    #
    # Copy our init scripts to /usr/local/share.
RUN mv -f /tmp/staging/application-start.sh /usr/local/share/ \
    && chmod +x /usr/local/share/application-start.sh \
    && chown ${USER_UID}:${USER_GID} /usr/local/share/application-start.sh 
    #
    # Set permissions for the workspace folder
WORKDIR /home/insuser/workspaces
COPY --from=publish /app/publish .
RUN mkdir -p  ${BASE_PATH} && chown -R ${USER_UID}:${USER_GID} ${BASE_PATH}
#
## FIX ME
EXPOSE 5000 5001
USER ${USERNAME}
ENTRYPOINT ["/usr/local/share/application-start.sh"]
CMD [ "/home/insuser/workspaces", "_NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.dll" ]
