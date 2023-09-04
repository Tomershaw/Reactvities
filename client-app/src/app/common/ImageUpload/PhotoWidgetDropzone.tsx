import {useCallback} from 'react'
import {useDropzone} from 'react-dropzone'
import { Header, Icon } from 'semantic-ui-react';

interface Props{
  setFiles:(files: object[]) => void;
}

export default  function PhotoWidgetDropzone({setFiles}:Props) {
 const dzStyle ={
  border:'dashed 3px #eee',
  borderColor:'#eee',
  borderRadius:'5px',
  paddingTop:'30px',
  textAlign: 'center',
  height:200  
 } as object

 const dzActive ={
  borderColor:'green'
 }


  const onDrop = useCallback((acceptedFiles:object[]) => {
    setFiles(acceptedFiles.map((file:object)=> Object.assign(file,{
      preview:URL.createObjectURL(file as Blob )
    })))
    console.log(acceptedFiles);
  }, [setFiles])
  const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

  return (
    <div {...getRootProps()} style={isDragActive ? {...dzStyle , ...dzActive} : dzStyle}>
      <input {...getInputProps()} />
      <Icon name='upload' size='huge' />
      <Header content='Drop image here ' />
    </div>
  )
}