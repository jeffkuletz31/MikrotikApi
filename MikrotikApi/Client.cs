using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MikrotikApi
{
    public class ResponseData : List<ResponseItem>
    {
    }

    public class ResponseItem : Dictionary<string, string>
    {
    }

    class ResponseDataFactory
    {
        public static ResponseData createResponseData(Protocol.Response response)
        {
            var responseData = new ResponseData();

            foreach (Protocol.Reply reply in response)
            {
                if (reply.Re)
                {
                    var responseItem = new ResponseItem();
                    foreach (Protocol.Word word in reply)
                    {
                        Protocol.Attribute attribute = word as Protocol.Attribute;
                        if (attribute != null)
                        {
                            responseItem[attribute.Name] = attribute.Value;
                        }
                    }

                    responseData.Add(responseItem);
                }
            }

            return responseData;
        }
    }

    public sealed class Client : IDisposable
    {
        private TcpClient _tcpClient;

        public Client (string host, int port = 8728)
        {
            _tcpClient = new TcpClient(host, port);
        }

        public void Login (string username, string password)
        {
            Protocol.Sentence initLogonMessage = new Protocol.Sentence();
            initLogonMessage.Add(new Protocol.Command("/login"));

            SendMessage(initLogonMessage);
            Protocol.Response initLogonResponse = ReceiveResponse();

            string challenge = ((Protocol.Attribute)initLogonResponse.First()[1]).Value;
            string response = EncodePassword(password, challenge);

            Protocol.Sentence completeLogonMessage = new Protocol.Sentence();
            completeLogonMessage.Add(new Protocol.Command("/login"));
            completeLogonMessage.Add(new Protocol.Attribute("name", username));
            completeLogonMessage.Add(new Protocol.Attribute("response", response));

            SendMessage(completeLogonMessage);
            Protocol.Response completeLogonResponse = ReceiveResponse();
            
            if (!completeLogonResponse.First().Done)
            {
                throw new Exception("Logon failed");
            }
        }

        public ResponseData DoCommand(string command)
        {
            Protocol.Sentence commandMessage = new Protocol.Sentence();
            commandMessage.Add(new Protocol.Command(command));
            SendMessage(commandMessage);

            return ResponseDataFactory.createResponseData(ReceiveResponse());
        }

        internal void Close()
        {
            _tcpClient.Close();
        }

        public string EncodePassword(string password, string challenge)
        {
            byte[] challenge_bytes = new byte[challenge.Length / 2];
            for (int i = 0; i <= challenge.Length - 2; i += 2)
            {
                challenge_bytes[i / 2] = Byte.Parse(challenge.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            byte[] input = new byte[1 + password.Length + challenge_bytes.Length];
            input[0] = 0;
            Encoding.ASCII.GetBytes(password.ToCharArray()).CopyTo(input, 1);
            challenge_bytes.CopyTo(input, 1 + password.Length);

            Byte[] output;
            System.Security.Cryptography.MD5 md5;

            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            output = md5.ComputeHash(input);

            //Convert encoded bytes back to a 'readable' string
            string response = "00";
            foreach (byte h in output)
            {
                response += h.ToString("x2");
            }
            return response;
        }

        private void SendMessage (Protocol.Sentence message)
        {
            byte[] buffer = message.Buffer.ToArray();

            _tcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }

        private Protocol.Response ReceiveResponse()
        {
            Protocol.Response response = new Protocol.Response();

            while(!response.Valid)
            {
                response.Add(ReceiveReply());
            }

            return response;
        }

        private Protocol.Reply ReceiveReply()
        {
            Protocol.Reply reply = new Protocol.Reply();

            while(!reply.Valid)
            {
                reply.Add(ReceiveWord());
            }

            return reply;
        }

        private Protocol.Word ReceiveWord()
        {
            int length = ReceiveLengthPrefix();
            byte[] buffer = new byte[length];
            string data;
            Protocol.Word word = null;

            if (length > 0)
            {
                _tcpClient.GetStream().Read(buffer, 0, length);
                data = Encoding.ASCII.GetString(buffer);

                if (data[0] == '=')
                {
                    List<String> pair = data.Substring(1).Split(new char[] { '=' }, 2).ToList();
                    word = new Protocol.Attribute(pair.First(), pair.Last());
                }
                else
                {
                    word = new Protocol.Word(data);
                }
            } else
            {
                word = new Protocol.Word("");
            }

            return word;
        }

        private int ReceiveLengthPrefix()
        {
            int l = _tcpClient.GetStream().ReadByte();

            if ((l & 0x80) == 0x80)
            {
                l = l ^ 0x80;
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
            }
            else if ((l & 0xC0) == 0xC0)
            {
                l = l ^ 0xC0;
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
            }
            else if ((l & 0xE0) == 0xE0)
            {
                l = l ^ 0xC0;
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
            }
            else if ((l & 0xF0) == 0xF0)
            {
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
                l += _tcpClient.GetStream().ReadByte();
                l = l << 8;
            }

            return l;
        }

        public void Dispose()
        {
            ((IDisposable)_tcpClient).Dispose();
        }
    }
}
