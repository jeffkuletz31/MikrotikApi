using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MikrotikApi.Protocol;

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
        public static ResponseData createResponseData(Response response)
        {
            var responseData = new ResponseData();

            foreach (ReplySentence reply in response)
            {
                if (reply.Re)
                {
                    var responseItem = new ResponseItem();
                    foreach (Word word in reply)
                    {
                        AttributeWord attribute = word as AttributeWord;
                        if (attribute != null)
                        {
                            responseItem[attribute.Key] = attribute.Value;
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

        public Client(string host, int port = 8728)
        {
            _tcpClient = new TcpClient(host, port);
        }

        public void Login(string username, string password)
        {
            Sentence initLogonMessage = new Sentence();
            initLogonMessage.Add(new CommandWord("/login"));

            SendMessage(initLogonMessage);
            Response initLogonResponse = ReceiveResponse();

            string challenge = ((AttributeWord)initLogonResponse.First()[1]).Value;
            string response = EncodePassword(password, challenge);

            Sentence completeLogonMessage = new Sentence();
            completeLogonMessage.Add(new CommandWord("/login"));
            completeLogonMessage.Add(new AttributeWord("name", username));
            completeLogonMessage.Add(new AttributeWord("response", response));

            SendMessage(completeLogonMessage);
            Response completeLogonResponse = ReceiveResponse();

            if (!completeLogonResponse.First().Done)
            {
                throw new Exception("Logon failed");
            }
        }

        public ResponseData DoCommand(string command)
        {
            Sentence commandMessage = new Sentence();
            commandMessage.Add(new CommandWord(command));
            SendMessage(commandMessage);

            return ResponseDataFactory.createResponseData(ReceiveResponse());
        }

        public ResponseData DoCommand(string command, Dictionary<string, string> attributes)
        {
            Sentence commandMessage = new Sentence();
            commandMessage.Add(new CommandWord(command));

            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                commandMessage.Add(new AttributeWord(attribute));
            }

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

        private void SendMessage(Sentence message)
        {
            byte[] buffer = message.Buffer.ToArray();

            _tcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }

        private Response ReceiveResponse()
        {
            Response response = new Response();

            while (!response.Valid)
            {
                response.Add(ReceiveReply());
            }

            return response;
        }

        private ReplySentence ReceiveReply()
        {
            ReplySentence reply = new ReplySentence();

            while (!reply.Valid)
            {
                reply.Add(ReceiveWord());
            }

            return reply;
        }

        private Word ReceiveWord()
        {
            int length = ReceiveLengthPrefix();
            byte[] buffer = new byte[length];
            string data;
            Word word = null;

            if (length > 0)
            {
                _tcpClient.GetStream().Read(buffer, 0, length);
                data = Encoding.ASCII.GetString(buffer);

                if (data[0] == '=')
                {
                    List<String> pair = data.Substring(1).Split(new char[] { '=' }, 2).ToList();
                    word = new AttributeWord(pair.First(), pair.Last());
                }
                else
                {
                    word = new Word(data);
                }
            }
            else
            {
                word = new Word("");
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
