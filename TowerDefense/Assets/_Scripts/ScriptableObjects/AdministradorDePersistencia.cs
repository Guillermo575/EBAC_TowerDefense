using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Administrador que guarda y carga los objetos de la clase persistente
 */
public class AdministradorDePersistencia : MonoBehaviour
{
    public List<ObjetoPersistente> ObjetosAGuardar;
    public void OnEnable()
    {
        CargarCambios();
    }
    public void OnDisable()
    {
        GuardarCambios();
    }
    public void CargarCambios()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Cargar();
        }
    }
    public void GuardarCambios()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Guardar();
        }
    }
}