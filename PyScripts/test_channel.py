import clr
import os
from pathlib import Path
from langchain.embeddings.openai import OpenAIEmbeddings
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.vectorstores.faiss import FAISS
from langchain import OpenAI, VectorDBQA

clr.AddReference("System")
def sayHi(message):
    print(message + ' ' + 'Say it')

def ReturnValue(api_key):
    os.environ['OPENAI_API_KEY'] = api_key
    txt = Path('knowledge/totem_knowledge.txt').read_text(encoding="utf8")
    text_splitter = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=0 )
    texts = text_splitter.split_text(txt)

    embeddings = OpenAIEmbeddings()
    vectorstore = FAISS.from_texts(texts, embeddings)

    qa = VectorDBQA.from_chain_type(llm=OpenAI(), chain_type="stuff", vectorstore=vectorstore)
    query = "What is Totem?"

    answer = qa.run(query)
    return answer
