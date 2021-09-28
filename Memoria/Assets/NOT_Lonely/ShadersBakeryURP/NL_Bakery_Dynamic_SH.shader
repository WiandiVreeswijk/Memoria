// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NOT_Lonely/URP/NL_Bakery_Dynamic_SH"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_BaseColor("BaseColor", Color) = (1,1,1,1)
		_BaseMap("Albedo", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_BumpScale("Normal Scale", Float) = 1
		_MetallicGlossMap("Metallic", 2D) = "black" {}
		[KeywordEnum(Metallic_Alpha,Albedo_Alpha)] _SmoothnessTextureChannel("Smoothness Source", Float) = 0
		_Smoothness("Smoothness Scale", Range( 0 , 1)) = 0.5
		_OcclusionMap("OcclusionMap", 2D) = "white" {}
		[KeywordEnum(OcclusionMap,Metallic_G_channel)] _AOSource("AO Source", Float) = 0
		_OcclusionStrength("Occlusion Strength", Range( 0 , 1)) = 1
		_Volume0("_Volume0", 3D) = "white" {}
		_Volume1("_Volume1", 3D) = "white" {}
		_Volume2("_Volume2", 3D) = "white" {}
		_VolumeMin("_VolumeMin", Vector) = (0,0,0,0)
		_VolumeInvSize("_VolumeInvSize", Vector) = (0,0,0,0)
		_BoostThreshold("BoostThreshold", Range( 0 , 1)) = 0
		_ShadowBoost("ShadowBoost", Range( 0 , 1)) = 0
		_Fresnelpower("Fresnel power", Range( 0 , 5)) = 3
		_Specpower("Spec power", Range( 0 , 5)) = 1
		_SubtractFresnelScale("Subtract Fresnel Scale", Range( 0 , 3)) = 1.1
		_SubtractFresnelPower("Subtract Fresnel  Power", Range( 0 , 20)) = 10
		_EmissionMap("Emission Map", 2D) = "black" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,1)
		[ASEEnd][Toggle]_UseEmissionMap("UseEmissionMap", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 3.5

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _ASE_BAKEDGI 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#pragma multi_compile_instancing
			#pragma shader_feature _SMOOTHNESSTEXTURECHANNEL_METALLIC_ALPHA _SMOOTHNESSTEXTURECHANNEL_ALBEDO_ALPHA
			#pragma shader_feature _AOSOURCE_OCCLUSIONMAP _AOSOURCE_METALLIC_G_CHANNEL


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _EmissionColor;
			float _BumpScale;
			float _Specpower;
			float _Smoothness;
			float _Fresnelpower;
			float _SubtractFresnelScale;
			float _SubtractFresnelPower;
			float _UseEmissionMap;
			float _OcclusionStrength;
			float _ShadowBoost;
			float _BoostThreshold;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BaseMap;
			sampler2D _BumpMap;
			sampler2D _MetallicGlossMap;
			sampler3D _Volume1;
			float3 _GlobalVolumeMin;
			float3 _GlobalVolumeInvSize;
			sampler3D _Volume0;
			sampler3D _Volume2;
			sampler2D _EmissionMap;
			sampler2D _OcclusionMap;
			UNITY_INSTANCING_BUFFER_START(NOT_LonelyURPNL_Bakery_Dynamic_SH)
				UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _BumpMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _MetallicGlossMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _EmissionMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _OcclusionMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float3, _VolumeInvSize)
				UNITY_DEFINE_INSTANCED_PROP(float3, _VolumeMin)
			UNITY_INSTANCING_BUFFER_END(NOT_LonelyURPNL_Bakery_Dynamic_SH)


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xyz = v.texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float4 _BaseMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_BaseMap_ST);
				float2 uv_BaseMap = IN.ase_texcoord7.xyz.xy * _BaseMap_ST_Instance.xy + _BaseMap_ST_Instance.zw;
				float4 tex2DNode45 = tex2D( _BaseMap, uv_BaseMap );
				float4 FinalAlbedo364 = ( _BaseColor * tex2DNode45 );
				
				float4 _BumpMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_BumpMap_ST);
				float2 uv_BumpMap = IN.ase_texcoord7.xyz.xy * _BumpMap_ST_Instance.xy + _BumpMap_ST_Instance.zw;
				float3 unpack46 = UnpackNormalScale( tex2D( _BumpMap, uv_BumpMap ), _BumpScale );
				unpack46.z = lerp( 1, unpack46.z, saturate(_BumpScale) );
				float3 tex2DNode46 = unpack46;
				float3 Normal348 = tex2DNode46;
				
				float4 _MetallicGlossMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_MetallicGlossMap_ST);
				float2 uv_MetallicGlossMap = IN.ase_texcoord7.xyz.xy * _MetallicGlossMap_ST_Instance.xy + _MetallicGlossMap_ST_Instance.zw;
				float4 tex2DNode49 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
				float M_Smoothness352 = tex2DNode49.a;
				float BC_Smoothness353 = tex2DNode45.a;
				#if defined(_SMOOTHNESSTEXTURECHANNEL_METALLIC_ALPHA)
				float staticSwitch51 = M_Smoothness352;
				#elif defined(_SMOOTHNESSTEXTURECHANNEL_ALBEDO_ALPHA)
				float staticSwitch51 = BC_Smoothness353;
				#else
				float staticSwitch51 = M_Smoothness352;
				#endif
				float3 temp_output_5_0_g42 = ddx( WorldNormal );
				float dotResult6_g42 = dot( temp_output_5_0_g42 , temp_output_5_0_g42 );
				float3 temp_output_2_0_g42 = ddy( WorldNormal );
				float dotResult3_g42 = dot( temp_output_2_0_g42 , temp_output_2_0_g42 );
				float Smoothness340 = ( 1.0 - max( ( 1.0 - ( _Smoothness * staticSwitch51 ) ) , pow( saturate( max( dotResult6_g42 , dotResult3_g42 ) ) , 0.333 ) ) );
				float temp_output_104_0 = ( 1.0 - Smoothness340 );
				float temp_output_109_0 = ( temp_output_104_0 * temp_output_104_0 );
				float temp_output_112_0 = ( temp_output_109_0 * temp_output_109_0 );
				float3 mapNormal16_g43 = tex2DNode46;
				float3 normalizeResult18_g43 = normalize( ( ( WorldTangent * mapNormal16_g43.x ) + ( WorldBiTangent * mapNormal16_g43.y ) + ( WorldNormal * mapNormal16_g43.z ) ) );
				float3 PerPixelNrml321 = normalizeResult18_g43;
				float3 _VolumeInvSize_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_VolumeInvSize);
				float3 temp_cast_1 = (1.0).xxx;
				float dotResult235 = dot( abs( _VolumeInvSize_Instance ) , temp_cast_1 );
				float3 _VolumeMin_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_VolumeMin);
				float3 UVs269 = ( ( WorldPosition - ( dotResult235 == 0.0 ? _GlobalVolumeMin : _VolumeMin_Instance ) ) * ( dotResult235 == 0.0 ? _GlobalVolumeInvSize : _VolumeInvSize_Instance ) );
				float4 tex3DNode16 = tex3D( _Volume1, UVs269 );
				float4 Volume1_RGB286 = tex3DNode16;
				float3 L1x304 = (Volume1_RGB286).rgb;
				float4 tex3DNode10 = tex3D( _Volume0, UVs269 );
				float4 Volume0_RGB272 = tex3DNode10;
				float3 L0305 = (Volume0_RGB272).rgb;
				float3 _lumaConv = float3(0.2125,0.7154,0.0721);
				float dotResult83 = dot( ( L1x304 / L0305 ) , _lumaConv );
				float4 tex3DNode17 = tex3D( _Volume2, UVs269 );
				float4 Volume2_RGB292 = tex3DNode17;
				float3 L1y303 = (Volume2_RGB292).rgb;
				float dotResult86 = dot( ( L1y303 / L0305 ) , _lumaConv );
				float Volume0_A276 = tex3DNode10.a;
				float Volume1_A284 = tex3DNode16.a;
				float Volume2_A296 = tex3DNode17.a;
				float3 appendResult22 = (float3(Volume0_A276 , Volume1_A284 , Volume2_A296));
				float3 L1z306 = appendResult22;
				float dotResult87 = dot( ( L1z306 / L0305 ) , _lumaConv );
				float3 appendResult88 = (float3(dotResult83 , dotResult86 , dotResult87));
				float3 normalizeResult94 = normalize( appendResult88 );
				float3 ViewDirection326 = WorldViewDirection;
				float3 normalizeResult98 = normalize( ( normalizeResult94 - ( ViewDirection326 * -1.0 ) ) );
				float dotResult101 = dot( PerPixelNrml321 , normalizeResult98 );
				float temp_output_100_0 = saturate( dotResult101 );
				float temp_output_117_0 = ( ( ( ( temp_output_112_0 * temp_output_100_0 ) - temp_output_100_0 ) * temp_output_100_0 ) + 1.0 );
				float3 SH319 = ( ( dotResult83 * L1x304 ) + ( dotResult86 * L1y303 ) + ( dotResult87 * L1z306 ) + L0305 );
				float3 temp_cast_2 = (0.0).xxx;
				float fresnelNdotV144 = dot( normalize( PerPixelNrml321 ), ViewDirection326 );
				float fresnelNode144 = ( 0.0 + 1.0 * pow( max( 1.0 - fresnelNdotV144 , 0.0001 ), _Fresnelpower ) );
				float fresnelNdotV149 = dot( normalize( PerPixelNrml321 ), ViewDirection326 );
				float fresnelNode149 = ( 0.0 + _SubtractFresnelScale * pow( max( 1.0 - fresnelNdotV149 , 0.0001 ), _SubtractFresnelPower ) );
				float SubtractFresnel334 = fresnelNode149;
				float clampResult175 = clamp( ( fresnelNode144 - SubtractFresnel334 ) , 0.0 , 1.0 );
				float4 temp_cast_4 = (1.0).xxxx;
				float4 _EmissionMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_EmissionMap_ST);
				float2 uv_EmissionMap = IN.ase_texcoord7.xyz.xy * _EmissionMap_ST_Instance.xy + _EmissionMap_ST_Instance.zw;
				float4 Emission330 = ( (( _UseEmissionMap )?( tex2D( _EmissionMap, uv_EmissionMap ) ):( temp_cast_4 )) * _EmissionColor );
				float4 FinalEmission332 = ( float4( ( ( _Specpower * max( ( ( ( temp_output_112_0 * 0.3183099 ) / ( ( temp_output_117_0 * temp_output_117_0 ) + 1E-07 ) ) * SH319 ) , temp_cast_2 ) ) * clampResult175 ) , 0.0 ) + Emission330 );
				
				float Metallic360 = tex2DNode49.r;
				
				float clampResult174 = clamp( ( Smoothness340 - SubtractFresnel334 ) , 0.0 , 1.0 );
				float FinalSmoothness338 = clampResult174;
				
				float4 _OcclusionMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_OcclusionMap_ST);
				float2 uv_OcclusionMap = IN.ase_texcoord7.xyz.xy * _OcclusionMap_ST_Instance.xy + _OcclusionMap_ST_Instance.zw;
				float OcclusionAO356 = tex2D( _OcclusionMap, uv_OcclusionMap ).g;
				float OcclusionM357 = tex2DNode49.g;
				#if defined(_AOSOURCE_OCCLUSIONMAP)
				float staticSwitch52 = OcclusionAO356;
				#elif defined(_AOSOURCE_METALLIC_G_CHANNEL)
				float staticSwitch52 = OcclusionM357;
				#else
				float staticSwitch52 = OcclusionAO356;
				#endif
				float lerpResult54 = lerp( 1.0 , staticSwitch52 , _OcclusionStrength);
				float FinalAO362 = lerpResult54;
				
				float Volume1_R282 = tex3DNode16.r;
				float Volume2_R293 = tex3DNode17.r;
				float3 break31 = L1z306;
				float3 appendResult30 = (float3(Volume1_R282 , Volume2_R293 , break31.x));
				float3 temp_output_7_0_g66 = ( 0.5 * appendResult30 );
				float Volume0_R273 = tex3DNode10.r;
				float temp_output_2_0_g66 = Volume0_R273;
				float temp_output_21_0_g66 = ( length( temp_output_7_0_g66 ) / temp_output_2_0_g66 );
				float temp_output_22_0_g66 = ( ( temp_output_21_0_g66 * 2.0 ) + 1.0 );
				float temp_output_28_0_g66 = ( ( 1.0 - temp_output_21_0_g66 ) / ( 1.0 + temp_output_21_0_g66 ) );
				float3 normalizeResult11_g66 = normalize( temp_output_7_0_g66 );
				float dotResult13_g66 = dot( normalizeResult11_g66 , PerPixelNrml321 );
				float Volume1_G285 = tex3DNode16.g;
				float Volume2_G294 = tex3DNode17.g;
				float3 appendResult36 = (float3(Volume1_G285 , Volume2_G294 , break31.y));
				float3 temp_output_7_0_g64 = ( 0.5 * appendResult36 );
				float Volume0_G274 = tex3DNode10.g;
				float temp_output_2_0_g64 = Volume0_G274;
				float temp_output_21_0_g64 = ( length( temp_output_7_0_g64 ) / temp_output_2_0_g64 );
				float temp_output_22_0_g64 = ( ( temp_output_21_0_g64 * 2.0 ) + 1.0 );
				float temp_output_28_0_g64 = ( ( 1.0 - temp_output_21_0_g64 ) / ( 1.0 + temp_output_21_0_g64 ) );
				float3 normalizeResult11_g64 = normalize( temp_output_7_0_g64 );
				float dotResult13_g64 = dot( normalizeResult11_g64 , PerPixelNrml321 );
				float Volume1_B283 = tex3DNode16.b;
				float Volume2_B295 = tex3DNode17.b;
				float3 appendResult41 = (float3(Volume1_B283 , Volume2_B295 , break31.z));
				float3 temp_output_7_0_g65 = ( 0.5 * appendResult41 );
				float Volume0_B275 = tex3DNode10.b;
				float temp_output_2_0_g65 = Volume0_B275;
				float temp_output_21_0_g65 = ( length( temp_output_7_0_g65 ) / temp_output_2_0_g65 );
				float temp_output_22_0_g65 = ( ( temp_output_21_0_g65 * 2.0 ) + 1.0 );
				float temp_output_28_0_g65 = ( ( 1.0 - temp_output_21_0_g65 ) / ( 1.0 + temp_output_21_0_g65 ) );
				float3 normalizeResult11_g65 = normalize( temp_output_7_0_g65 );
				float dotResult13_g65 = dot( normalizeResult11_g65 , PerPixelNrml321 );
				float4 appendResult28 = (float4(( ( ( ( ( temp_output_22_0_g66 + 1.0 ) * ( 1.0 - temp_output_28_0_g66 ) ) * pow( ( ( dotResult13_g66 * 0.5 ) + 0.5 ) , temp_output_22_0_g66 ) ) + temp_output_28_0_g66 ) * temp_output_2_0_g66 ) , ( ( ( ( ( temp_output_22_0_g64 + 1.0 ) * ( 1.0 - temp_output_28_0_g64 ) ) * pow( ( ( dotResult13_g64 * 0.5 ) + 0.5 ) , temp_output_22_0_g64 ) ) + temp_output_28_0_g64 ) * temp_output_2_0_g64 ) , ( ( ( ( ( temp_output_22_0_g65 + 1.0 ) * ( 1.0 - temp_output_28_0_g65 ) ) * pow( ( ( dotResult13_g65 * 0.5 ) + 0.5 ) , temp_output_22_0_g65 ) ) + temp_output_28_0_g65 ) * temp_output_2_0_g65 ) , 0.0));
				float4 GI345 = appendResult28;
				float grayscale254 = dot(GI345.rgb, float3(0.299,0.587,0.114));
				float smoothstepResult216 = smoothstep( (-20.0 + (_ShadowBoost - 0.0) * (0.0 - -20.0) / (1.0 - 0.0)) , 1.0 , saturate( ( grayscale254 / _BoostThreshold ) ));
				float4 FinalGI343 = ( GI345 + ( 1.0 - smoothstepResult216 ) );
				
				float3 Albedo = FinalAlbedo364.rgb;
				float3 Normal = Normal348;
				float3 Emission = FinalEmission332.rgb;
				float3 Specular = 0.5;
				float Metallic = Metallic360;
				float Smoothness = FinalSmoothness338;
				float Occlusion = FinalAO362;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = FinalGI343.rgb;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _ASE_BAKEDGI 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _EmissionColor;
			float _BumpScale;
			float _Specpower;
			float _Smoothness;
			float _Fresnelpower;
			float _SubtractFresnelScale;
			float _SubtractFresnelPower;
			float _UseEmissionMap;
			float _OcclusionStrength;
			float _ShadowBoost;
			float _BoostThreshold;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			UNITY_INSTANCING_BUFFER_START(NOT_LonelyURPNL_Bakery_Dynamic_SH)
			UNITY_INSTANCING_BUFFER_END(NOT_LonelyURPNL_Bakery_Dynamic_SH)


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _ASE_BAKEDGI 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile_instancing
			#pragma shader_feature _SMOOTHNESSTEXTURECHANNEL_METALLIC_ALPHA _SMOOTHNESSTEXTURECHANNEL_ALBEDO_ALPHA


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _EmissionColor;
			float _BumpScale;
			float _Specpower;
			float _Smoothness;
			float _Fresnelpower;
			float _SubtractFresnelScale;
			float _SubtractFresnelPower;
			float _UseEmissionMap;
			float _OcclusionStrength;
			float _ShadowBoost;
			float _BoostThreshold;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _BaseMap;
			sampler2D _MetallicGlossMap;
			sampler2D _BumpMap;
			sampler3D _Volume1;
			float3 _GlobalVolumeMin;
			float3 _GlobalVolumeInvSize;
			sampler3D _Volume0;
			sampler3D _Volume2;
			sampler2D _EmissionMap;
			UNITY_INSTANCING_BUFFER_START(NOT_LonelyURPNL_Bakery_Dynamic_SH)
				UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _MetallicGlossMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _BumpMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _EmissionMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float3, _VolumeInvSize)
				UNITY_DEFINE_INSTANCED_PROP(float3, _VolumeMin)
			UNITY_INSTANCING_BUFFER_END(NOT_LonelyURPNL_Bakery_Dynamic_SH)


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord5.xyz = ase_worldBitangent;
				
				o.ase_texcoord2.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_tangent = v.ase_tangent;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 _BaseMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_BaseMap_ST);
				float2 uv_BaseMap = IN.ase_texcoord2.xyz.xy * _BaseMap_ST_Instance.xy + _BaseMap_ST_Instance.zw;
				float4 tex2DNode45 = tex2D( _BaseMap, uv_BaseMap );
				float4 FinalAlbedo364 = ( _BaseColor * tex2DNode45 );
				
				float4 _MetallicGlossMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_MetallicGlossMap_ST);
				float2 uv_MetallicGlossMap = IN.ase_texcoord2.xyz.xy * _MetallicGlossMap_ST_Instance.xy + _MetallicGlossMap_ST_Instance.zw;
				float4 tex2DNode49 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
				float M_Smoothness352 = tex2DNode49.a;
				float BC_Smoothness353 = tex2DNode45.a;
				#if defined(_SMOOTHNESSTEXTURECHANNEL_METALLIC_ALPHA)
				float staticSwitch51 = M_Smoothness352;
				#elif defined(_SMOOTHNESSTEXTURECHANNEL_ALBEDO_ALPHA)
				float staticSwitch51 = BC_Smoothness353;
				#else
				float staticSwitch51 = M_Smoothness352;
				#endif
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float3 temp_output_5_0_g42 = ddx( ase_worldNormal );
				float dotResult6_g42 = dot( temp_output_5_0_g42 , temp_output_5_0_g42 );
				float3 temp_output_2_0_g42 = ddy( ase_worldNormal );
				float dotResult3_g42 = dot( temp_output_2_0_g42 , temp_output_2_0_g42 );
				float Smoothness340 = ( 1.0 - max( ( 1.0 - ( _Smoothness * staticSwitch51 ) ) , pow( saturate( max( dotResult6_g42 , dotResult3_g42 ) ) , 0.333 ) ) );
				float temp_output_104_0 = ( 1.0 - Smoothness340 );
				float temp_output_109_0 = ( temp_output_104_0 * temp_output_104_0 );
				float temp_output_112_0 = ( temp_output_109_0 * temp_output_109_0 );
				float3 ase_worldTangent = IN.ase_texcoord4.xyz;
				float4 _BumpMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_BumpMap_ST);
				float2 uv_BumpMap = IN.ase_texcoord2.xyz.xy * _BumpMap_ST_Instance.xy + _BumpMap_ST_Instance.zw;
				float3 unpack46 = UnpackNormalScale( tex2D( _BumpMap, uv_BumpMap ), _BumpScale );
				unpack46.z = lerp( 1, unpack46.z, saturate(_BumpScale) );
				float3 tex2DNode46 = unpack46;
				float3 mapNormal16_g43 = tex2DNode46;
				float3 ase_worldBitangent = IN.ase_texcoord5.xyz;
				float3 normalizeResult18_g43 = normalize( ( ( ase_worldTangent * mapNormal16_g43.x ) + ( ase_worldBitangent * mapNormal16_g43.y ) + ( ase_worldNormal * mapNormal16_g43.z ) ) );
				float3 PerPixelNrml321 = normalizeResult18_g43;
				float3 _VolumeInvSize_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_VolumeInvSize);
				float3 temp_cast_1 = (1.0).xxx;
				float dotResult235 = dot( abs( _VolumeInvSize_Instance ) , temp_cast_1 );
				float3 _VolumeMin_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_VolumeMin);
				float3 UVs269 = ( ( WorldPosition - ( dotResult235 == 0.0 ? _GlobalVolumeMin : _VolumeMin_Instance ) ) * ( dotResult235 == 0.0 ? _GlobalVolumeInvSize : _VolumeInvSize_Instance ) );
				float4 tex3DNode16 = tex3D( _Volume1, UVs269 );
				float4 Volume1_RGB286 = tex3DNode16;
				float3 L1x304 = (Volume1_RGB286).rgb;
				float4 tex3DNode10 = tex3D( _Volume0, UVs269 );
				float4 Volume0_RGB272 = tex3DNode10;
				float3 L0305 = (Volume0_RGB272).rgb;
				float3 _lumaConv = float3(0.2125,0.7154,0.0721);
				float dotResult83 = dot( ( L1x304 / L0305 ) , _lumaConv );
				float4 tex3DNode17 = tex3D( _Volume2, UVs269 );
				float4 Volume2_RGB292 = tex3DNode17;
				float3 L1y303 = (Volume2_RGB292).rgb;
				float dotResult86 = dot( ( L1y303 / L0305 ) , _lumaConv );
				float Volume0_A276 = tex3DNode10.a;
				float Volume1_A284 = tex3DNode16.a;
				float Volume2_A296 = tex3DNode17.a;
				float3 appendResult22 = (float3(Volume0_A276 , Volume1_A284 , Volume2_A296));
				float3 L1z306 = appendResult22;
				float dotResult87 = dot( ( L1z306 / L0305 ) , _lumaConv );
				float3 appendResult88 = (float3(dotResult83 , dotResult86 , dotResult87));
				float3 normalizeResult94 = normalize( appendResult88 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 ViewDirection326 = ase_worldViewDir;
				float3 normalizeResult98 = normalize( ( normalizeResult94 - ( ViewDirection326 * -1.0 ) ) );
				float dotResult101 = dot( PerPixelNrml321 , normalizeResult98 );
				float temp_output_100_0 = saturate( dotResult101 );
				float temp_output_117_0 = ( ( ( ( temp_output_112_0 * temp_output_100_0 ) - temp_output_100_0 ) * temp_output_100_0 ) + 1.0 );
				float3 SH319 = ( ( dotResult83 * L1x304 ) + ( dotResult86 * L1y303 ) + ( dotResult87 * L1z306 ) + L0305 );
				float3 temp_cast_2 = (0.0).xxx;
				float fresnelNdotV144 = dot( normalize( PerPixelNrml321 ), ViewDirection326 );
				float fresnelNode144 = ( 0.0 + 1.0 * pow( max( 1.0 - fresnelNdotV144 , 0.0001 ), _Fresnelpower ) );
				float fresnelNdotV149 = dot( normalize( PerPixelNrml321 ), ViewDirection326 );
				float fresnelNode149 = ( 0.0 + _SubtractFresnelScale * pow( max( 1.0 - fresnelNdotV149 , 0.0001 ), _SubtractFresnelPower ) );
				float SubtractFresnel334 = fresnelNode149;
				float clampResult175 = clamp( ( fresnelNode144 - SubtractFresnel334 ) , 0.0 , 1.0 );
				float4 temp_cast_4 = (1.0).xxxx;
				float4 _EmissionMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(NOT_LonelyURPNL_Bakery_Dynamic_SH,_EmissionMap_ST);
				float2 uv_EmissionMap = IN.ase_texcoord2.xyz.xy * _EmissionMap_ST_Instance.xy + _EmissionMap_ST_Instance.zw;
				float4 Emission330 = ( (( _UseEmissionMap )?( tex2D( _EmissionMap, uv_EmissionMap ) ):( temp_cast_4 )) * _EmissionColor );
				float4 FinalEmission332 = ( float4( ( ( _Specpower * max( ( ( ( temp_output_112_0 * 0.3183099 ) / ( ( temp_output_117_0 * temp_output_117_0 ) + 1E-07 ) ) * SH319 ) , temp_cast_2 ) ) * clampResult175 ) , 0.0 ) + Emission330 );
				
				
				float3 Albedo = FinalAlbedo364.rgb;
				float3 Emission = FinalEmission332.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

	
	}
	/*ase_lod*/
	CustomEditor "NL_Bakery_Dynamic_SH_GUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18712
-1920;139;1920;989;598.197;1519.403;2.415653;True;False
Node;AmplifyShaderEditor.CommentaryNode;268;-1851.338,-274.2881;Inherit;False;1756.636;916.1499;3D-texture UVs;11;269;246;242;12;244;11;233;239;237;240;243;3D-texture UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;243;-1801.338,63.58705;Inherit;False;554.2419;334.1755;IsGlobal;4;234;236;235;232;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;232;-1753.639,112.287;Inherit;False;InstancedProperty;_VolumeInvSize;_VolumeInvSize;16;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;236;-1554.099,281.762;Inherit;False;Constant;_Float10;Float 10;25;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;234;-1541.962,153.0909;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;237;-1070.046,152.6166;Inherit;False;Constant;_Float11;Float 11;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;235;-1399.099,165.7623;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;239;-1166.624,319.2283;Inherit;False;Global;_GlobalVolumeMin;_GlobalVolumeMin;17;0;Create;True;0;0;0;False;0;False;0,0,0;-0.01728868,0.06700015,-3.692151;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;240;-1127.179,480.9278;Inherit;False;InstancedProperty;_VolumeMin;_VolumeMin;15;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Compare;233;-892.4454,218.3172;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;244;-1170.459,-43.33603;Inherit;False;Global;_GlobalVolumeInvSize;_GlobalVolumeInvSize;18;0;Create;True;0;0;0;False;0;False;0,0,0;0.2603163,0.4176401,0.1681614;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;11;-883.4161,-224.2881;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-649.0675,-57.18802;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Compare;242;-894.6285,13.29118;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;-485.1954,18.70207;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;271;27.73331,-958.0974;Inherit;False;895.0005;1603.905;3D-textures Inputs;19;275;273;294;293;295;283;285;282;274;296;292;284;286;272;276;17;10;16;270;3D-textures Inputs;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;269;-308.2463,13.84337;Inherit;False;UVs;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;270;56.54825,-196.5516;Inherit;False;269;UVs;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;16;299.8812,-214.2477;Inherit;True;Property;_Volume1;_Volume1;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;LockedToTexture3D;False;Object;-1;Auto;Texture3D;8;0;SAMPLER3D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;305.2711,-745.6891;Inherit;True;Property;_Volume0;_Volume0;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;LockedToTexture3D;False;Object;-1;Auto;Texture3D;8;0;SAMPLER3D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;299.8812,313.7512;Inherit;True;Property;_Volume2;_Volume2;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;LockedToTexture3D;False;Object;-1;Auto;Texture3D;8;0;SAMPLER3D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;286;697.7892,-344.8687;Inherit;False;Volume1_RGB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;70;976.5934,191.9004;Inherit;False;751.6001;181.2001;L1x;3;69;287;304;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;302;1006.476,-486.3405;Inherit;False;610.9312;319.2587;L1z;5;306;277;301;291;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;73;980.1188,443.4622;Inherit;False;759.3;179.6001;L1y;3;72;297;303;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;276;688.2721,-523.4999;Inherit;False;Volume0_A;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;284;694.3842,-6.347662;Inherit;False;Volume1_A;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;296;690.6752,495.2662;Inherit;False;Volume2_A;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;292;694.0802,156.7453;Inherit;False;Volume2_RGB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;68;979.2499,-55.77551;Inherit;False;749.2;178.4;L0;3;67;281;305;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;272;691.6771,-862.0209;Inherit;False;Volume0_RGB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;301;1031.476,-256.9835;Inherit;False;296;Volume2_A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;287;991.2843,241.2444;Inherit;False;286;Volume1_RGB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;281;997.4908,-8.760504;Inherit;False;272;Volume0_RGB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;297;999.11,492.0547;Inherit;False;292;Volume2_RGB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;1033.919,-347.3295;Inherit;False;284;Volume1_A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;277;1033.845,-434.3405;Inherit;False;276;Volume0_A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;367;1927.11,-1228.523;Inherit;False;985.4373;1208.966;Texture Maps Inputs;16;364;353;48;47;45;59;348;321;184;46;356;357;360;352;50;49;Texture Maps Inputs;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;67;1244.25,-7.775518;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;342;-415.0074,1947.22;Inherit;False;6021.703;1413.913;SpecularSH;47;76;78;80;89;142;108;99;323;110;113;101;103;114;132;115;119;120;121;169;126;135;137;176;177;145;258;332;97;83;87;86;326;141;94;96;116;118;124;122;123;320;335;136;171;175;331;318;SpecularSH;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;72;1250.419,491.4626;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;1243.443,-363.2174;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;45;2160.851,-974.7418;Inherit;True;Property;_BaseMap;Albedo;3;0;Create;False;0;0;0;False;0;False;-1;None;804e29c8eccaad845bbcccb753b027f5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;49;2148.645,-449.6819;Inherit;True;Property;_MetallicGlossMap;Metallic;6;0;Create;False;0;0;0;False;0;False;-1;None;2e6a6d11012ab554bb3abce4cd668bc4;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;69;1248.193,239.8994;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;305;1517.989,-2.300542;Inherit;False;L0;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;337;1900.821,723.1718;Inherit;False;1904.971;408.2571;Smoothness;11;354;173;336;338;174;340;230;57;56;51;355;Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;76;-363.1852,2137.534;Inherit;False;424.2106;186.111;nL1x;3;75;310;311;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;353;2485.055,-876.2343;Inherit;False;BC_Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;303;1525.962,493.3606;Inherit;False;L1y;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;304;1516.378,245.3744;Inherit;False;L1x;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;352;2492.709,-319.7298;Inherit;False;M_Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;80;-365.0074,2576.432;Inherit;False;426.4327;183.8892;nL1z;3;308;79;313;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;78;-365.0074,2368.433;Inherit;False;426.4327;185;nL1y;3;77;312;316;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;306;1421.735,-371.8199;Inherit;False;L1z;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;308;-347.7802,2614.232;Inherit;False;306;L1z;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;318;78.75083,1997.22;Inherit;False;282;238;Bakery constant;1;139;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;310;-339.839,2242.446;Inherit;False;305;L0;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;312;-352.0218,2477.469;Inherit;False;305;L0;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;354;1949.367,965.4059;Inherit;False;353;BC_Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;355;1952.629,880.3519;Inherit;False;352;M_Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;311;-341.839,2170.447;Inherit;False;304;L1x;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;316;-351.6137,2405.428;Inherit;False;303;L1y;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;313;-346.0218,2685.469;Inherit;False;305;L0;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;75;-92.97455,2185.534;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;77;-92.57483,2416.433;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;139;128.7508,2047.22;Inherit;False;Constant;_lumaConv;lumaConv;18;0;Create;True;0;0;0;False;0;False;0.2125,0.7154,0.0721;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;56;2195.854,773.1718;Inherit;False;Property;_Smoothness;Smoothness Scale;8;0;Create;False;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;79;-92.57483,2624.432;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;51;2184.367,906.7886;Inherit;False;Property;_SmoothnessTextureChannel;Smoothness Source;7;0;Create;False;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;Metallic_Alpha;Albedo_Alpha;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;2487.365,850.4459;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;89;583.4176,2368.971;Inherit;False;211;209;dominantDir;1;88;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;86;410.4085,2450.033;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;87;410.8392,2570.635;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;83;411.5564,2309.059;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;97;811.2407,2612.569;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;59;1952.029,-681.1753;Inherit;False;Property;_BumpScale;Normal Scale;5;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;2155.904,-733.7673;Inherit;True;Property;_BumpMap;NormalMap;4;0;Create;False;0;0;0;False;0;False;-1;None;245b9cef5b20bd945a486a2dd5fa80c8;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;230;2641.476,850.4918;Inherit;False;GeometricSpecularAA;-1;;42;9cfafae1c29d1a14aa62f5289dc60299;0;1;13;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;326;994.601,2612.814;Inherit;False;ViewDirection;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;633.4182,2418.971;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;142;1044.143,2747.954;Inherit;False;Constant;_Float8;Float 8;17;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;94;1230.057,2500.046;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;340;3028.03,851.1225;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;184;2472.772,-731.2908;Inherit;False;PerPixelNormal;0;;43;8d86515a1b400c8408ed7274c4002518;0;1;15;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;108;1370.161,2164.979;Inherit;False;421;154;perceptualRoughness;2;341;104;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;1236.441,2611.628;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;1381.88,2207.221;Inherit;False;340;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;99;1589.751,2454.154;Inherit;False;225;161;halfDir;1;98;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;96;1416.74,2501.842;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;321;2692.324,-737.4939;Inherit;False;PerPixelNrml;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;98;1639.751,2504.154;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;323;1647.138,2358.66;Inherit;False;321;PerPixelNrml;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;110;1846.102,2159.862;Inherit;False;212;185;roughness;1;109;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;104;1612.161,2214.978;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;113;2103.196,2157.408;Inherit;False;212;185;a2;1;112;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;101;1906.259,2461.198;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;103;2102.05,2420.326;Inherit;False;215;161;nh;1;100;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;1896.102,2209.862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;100;2152.05,2470.326;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;2153.196,2207.408;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;2464.672,2451.582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;2673.672,2518.582;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;132;436.2609,2870.705;Inherit;False;897.1388;440.8992;SH;9;317;315;309;131;128;129;130;314;319;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;315;458.9997,2942.814;Inherit;False;304;L1x;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;118;2859.672,2734.582;Inherit;False;Constant;_Float5;Float 5;18;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;460.8266,3038.218;Inherit;False;303;L1y;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;2846.672,2594.582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;119;3017.672,2566.582;Inherit;False;202;185;d;1;117;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;309;464.1147,3138.608;Inherit;False;306;L1z;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;3067.672,2616.582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;314;720.3579,3236.23;Inherit;False;305;L0;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;750.658,3119.278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;745.8494,3018.324;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;750.3898,2917.927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;267;3888.645,640.6581;Inherit;False;925.0613;475.8484;Simple fresnel hack to remove very thin highlights that appear on a very high view angle to a surface;6;334;149;327;161;160;324;Fresnel Subtraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;327;3952.645,784.6583;Inherit;False;326;ViewDirection;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;160;3936.645,880.6582;Inherit;False;Property;_SubtractFresnelScale;Subtract Fresnel Scale;21;0;Create;False;0;0;0;False;0;False;1.1;1.1;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;324;3952.645,704.6583;Inherit;False;321;PerPixelNrml;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;124;3360.185,2766.604;Inherit;False;Constant;_Float6;Float 6;18;0;Create;True;0;0;0;False;0;False;1E-07;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;3282.671,2424.18;Inherit;False;Constant;_UNITY_INV_PI;UNITY_INV_PI;18;0;Create;True;0;0;0;False;0;False;0.3183099;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;3936.645,1024.658;Inherit;False;Property;_SubtractFresnelPower;Subtract Fresnel  Power;22;0;Create;True;0;0;0;False;0;False;10;10;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;3350.678,2604.305;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;1012.515,3019.343;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;3521.371,2348.781;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;149;4272.645,688.6583;Inherit;True;Standard;WorldNormal;ViewDir;True;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;329;1914.195,1335.779;Inherit;False;1230.879;483.0181;Emission;6;257;256;264;265;255;330;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;319;1140.977,3014.206;Inherit;False;SH;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;126;3716.529,2413.721;Inherit;False;202;185;spec;1;125;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;169;3549.083,2940.635;Inherit;False;675.9998;313;Fresnel;4;325;144;178;328;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;3557.279,2613.405;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;178;3567.186,3171.42;Inherit;False;Property;_Fresnelpower;Fresnel power;19;0;Create;True;0;0;0;False;0;False;3;3;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;334;4624.645,672.6583;Inherit;False;SubtractFresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;255;1964.195,1493.503;Inherit;True;Property;_EmissionMap;Emission Map;23;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;265;2133.053,1385.779;Inherit;False;Constant;_Float12;Float 12;25;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;328;3627.457,3071.559;Inherit;False;326;ViewDirection;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;325;3627.595,2980.789;Inherit;False;321;PerPixelNrml;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;320;3775.018,2618.343;Inherit;False;319;SH;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;125;3766.529,2463.721;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;4049.584,2628.033;Inherit;False;Constant;_Float7;Float 7;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;144;3903.081,2990.635;Inherit;True;Standard;WorldNormal;ViewDir;True;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;137;4273.974,2463.771;Inherit;False;202;185;lightmapSpec;1;134;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;4022.704,2488.979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;264;2335.215,1467.53;Inherit;False;Property;_UseEmissionMap;UseEmissionMap;25;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;335;4248.048,3067.864;Inherit;False;334;SubtractFresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;256;2355.634,1603.797;Inherit;False;Property;_EmissionColor;Emission Color;24;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;2676.073,1498.773;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;171;4477.087,2989.872;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;4558.137,2438.788;Inherit;False;Property;_Specpower;Spec power;20;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;134;4323.974,2513.771;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;330;2883.143,1495.304;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;175;4823.55,2617.028;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;4880.599,2496.432;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;5051.065,2527.565;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;331;5009.12,2650.295;Inherit;False;330;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;47;2260.929,-1178.744;Inherit;False;Property;_BaseColor;BaseColor;2;0;Create;False;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;351;-414.0297,854.5683;Inherit;False;2202.679;955.5751;DiffuseSH;20;35;40;33;345;28;36;30;288;280;279;289;41;300;278;307;31;299;322;290;298;DiffuseSH;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;258;5227.468,2531.857;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;247;3223.119,1283.359;Inherit;False;1806.154;541.0466;Helpful for doors, when you have too dark lighting inside one of rooms and the door looks not realistic when it opens into the side of this room.;14;346;343;228;224;229;254;216;186;222;251;223;248;347;0;Shadow boost;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;2514.473,-998.2688;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;332;5382.696,2530.282;Inherit;False;FinalEmission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;33;736.9025,904.5683;Inherit;False;338;209;color.r;1;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;248;3335.918,1634.235;Inherit;False;350;166;Threshold to start boost dark areas from;1;214;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;364;2691.207,-1004.991;Inherit;False;FinalAlbedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;35;726.8459,1230.251;Inherit;False;338;209;color.g;1;38;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;350;1908.66,186.1938;Inherit;False;1043.118;386.3043;Occlusion;11;362;359;358;2;5;3;4;53;52;54;55;Occlusion;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;40;723.3425,1524.995;Inherit;False;338;209;color.b;1;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;52;2154.453,324.85;Inherit;False;Property;_AOSource;AO Source;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;OcclusionMap;Metallic_G_channel;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;2200.669,452.3666;Inherit;False;Property;_OcclusionStrength;Occlusion Strength;11;0;Create;False;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;358;1933.508,306.7987;Inherit;False;356;OcclusionAO;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;300;57.64159,1694.144;Inherit;False;295;Volume2_B;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;37;781.6605,951.9473;Inherit;False;shEvaluateDiffuseL1Geom;-1;;66;3802526d982db294db2cd7ebe076d09b;0;3;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;362;2755.285,361.3596;Inherit;False;FinalAO;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;347;4490.301,1321.579;Inherit;False;345;GI;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;3244.88,1333.377;Inherit;False;345;GI;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;343;4834.766,1337.196;Inherit;False;FinalGI;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;216;4313.381,1404.326;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;186;3518.581,1484.484;Inherit;False;Property;_ShadowBoost;ShadowBoost;18;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;251;3836.896,1473.905;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-20;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;223;3689.725,1326.997;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;38;779.8458,1284.251;Inherit;False;shEvaluateDiffuseL1Geom;-1;;64;3802526d982db294db2cd7ebe076d09b;0;3;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;54;2575.618,364.2989;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;282;695.2682,-249.3265;Inherit;False;Volume1_R;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;173;3262.732,890.1882;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;3387.918,1687.235;Inherit;False;Property;_BoostThreshold;BoostThreshold;17;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;278;319.1977,1494.854;Inherit;False;275;Volume0_B;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;307;-364.0297,1320.325;Inherit;False;306;L1z;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;31;-159.2496,1322.337;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;299;40.42326,1323.187;Inherit;False;294;Volume2_G;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;491.5864,1423.73;Inherit;False;321;PerPixelNrml;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;55;2377.417,236.1937;Inherit;False;Constant;_Float4;Float 4;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;39;773.3423,1574.995;Inherit;False;shEvaluateDiffuseL1Geom;-1;;65;3802526d982db294db2cd7ebe076d09b;0;3;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;298;35.12462,1108.831;Inherit;False;293;Volume2_R;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;366;5714.831,1214.902;Inherit;False;364;FinalAlbedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;364.9508,1597.254;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;174;3405.616,892.2566;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;338;3579.84,888.8896;Inherit;False;FinalSmoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;3046.345,991.3685;Inherit;False;334;SubtractFresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;290;60.09477,1599.91;Inherit;False;283;Volume1_B;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;222;3859.725,1335.997;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;293;691.5592,252.2872;Inherit;False;Volume2_R;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;229;4509.106,1401.772;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;344;5733.874,1711.474;Inherit;False;343;FinalGI;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;333;5710.426,1381.049;Inherit;False;332;FinalEmission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;361;5730.751,1464.225;Inherit;False;360;Metallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;363;5733.434,1629.482;Inherit;False;362;FinalAO;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;339;5691.769,1548.485;Inherit;False;338;FinalSmoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;283;694.3842,-85.34766;Inherit;False;Volume1_B;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;356;2491.078,-174.9834;Inherit;False;OcclusionAO;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;357;2491.303,-400.8201;Inherit;False;OcclusionM;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;360;2490.326,-484.6038;Inherit;False;Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;2153.966,-222.3604;Inherit;True;Property;_OcclusionMap;OcclusionMap;9;0;Create;False;0;0;0;False;0;False;-1;None;2eefe8dc4d786fa40aee29bb956f4a3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;273;689.1561,-766.479;Inherit;False;Volume0_R;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;285;690.0382,-169.5466;Inherit;False;Volume1_G;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;274;683.9252,-686.6985;Inherit;False;Volume0_G;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;254;3425.093,1333.779;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;295;691.6752,416.2662;Inherit;False;Volume2_B;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;294;686.3282,332.0673;Inherit;False;Volume2_G;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;4667.132,1333.359;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;224;3850.725,1708.999;Inherit;False;Constant;_Float9;Float 9;24;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;345;1564.649,1250.657;Inherit;False;GI;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;1281.192,1255.975;Inherit;True;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;352.5416,1341.261;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;337.6546,1028.046;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;275;688.2721,-602.5001;Inherit;False;Volume0_B;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;288;38.78166,1018.16;Inherit;False;282;Volume1_R;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;349;5727.062,1296.962;Inherit;False;348;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;41.53276,1216.15;Inherit;False;285;Volume1_G;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;359;1939.134,399.0691;Inherit;False;357;OcclusionM;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;338.8201,1213.735;Inherit;False;274;Volume0_G;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;348;2479.653,-640.8023;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;280;349.827,931.5866;Inherit;False;273;Volume0_R;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;6007.684,1290.297;Float;False;True;-1;2;NL_Bakery_Dynamic_SH_GUI;0;2;NOT_Lonely/URP/NL_Bakery_Dynamic_SH;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;17;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;3;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;36;Workflow;1;Surface;0;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;1;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;False;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3310.1,1300.21;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;5;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;234;0;232;0
WireConnection;235;0;234;0
WireConnection;235;1;236;0
WireConnection;233;0;235;0
WireConnection;233;1;237;0
WireConnection;233;2;239;0
WireConnection;233;3;240;0
WireConnection;12;0;11;0
WireConnection;12;1;233;0
WireConnection;242;0;235;0
WireConnection;242;1;237;0
WireConnection;242;2;244;0
WireConnection;242;3;232;0
WireConnection;246;0;12;0
WireConnection;246;1;242;0
WireConnection;269;0;246;0
WireConnection;16;1;270;0
WireConnection;10;1;270;0
WireConnection;17;1;270;0
WireConnection;286;0;16;0
WireConnection;276;0;10;4
WireConnection;284;0;16;4
WireConnection;296;0;17;4
WireConnection;292;0;17;0
WireConnection;272;0;10;0
WireConnection;67;0;281;0
WireConnection;72;0;297;0
WireConnection;22;0;277;0
WireConnection;22;1;291;0
WireConnection;22;2;301;0
WireConnection;69;0;287;0
WireConnection;305;0;67;0
WireConnection;353;0;45;4
WireConnection;303;0;72;0
WireConnection;304;0;69;0
WireConnection;352;0;49;4
WireConnection;306;0;22;0
WireConnection;75;0;311;0
WireConnection;75;1;310;0
WireConnection;77;0;316;0
WireConnection;77;1;312;0
WireConnection;79;0;308;0
WireConnection;79;1;313;0
WireConnection;51;1;355;0
WireConnection;51;0;354;0
WireConnection;57;0;56;0
WireConnection;57;1;51;0
WireConnection;86;0;77;0
WireConnection;86;1;139;0
WireConnection;87;0;79;0
WireConnection;87;1;139;0
WireConnection;83;0;75;0
WireConnection;83;1;139;0
WireConnection;46;5;59;0
WireConnection;230;13;57;0
WireConnection;326;0;97;0
WireConnection;88;0;83;0
WireConnection;88;1;86;0
WireConnection;88;2;87;0
WireConnection;94;0;88;0
WireConnection;340;0;230;0
WireConnection;184;15;46;0
WireConnection;141;0;326;0
WireConnection;141;1;142;0
WireConnection;96;0;94;0
WireConnection;96;1;141;0
WireConnection;321;0;184;0
WireConnection;98;0;96;0
WireConnection;104;0;341;0
WireConnection;101;0;323;0
WireConnection;101;1;98;0
WireConnection;109;0;104;0
WireConnection;109;1;104;0
WireConnection;100;0;101;0
WireConnection;112;0;109;0
WireConnection;112;1;109;0
WireConnection;114;0;112;0
WireConnection;114;1;100;0
WireConnection;115;0;114;0
WireConnection;115;1;100;0
WireConnection;116;0;115;0
WireConnection;116;1;100;0
WireConnection;117;0;116;0
WireConnection;117;1;118;0
WireConnection;130;0;87;0
WireConnection;130;1;309;0
WireConnection;129;0;86;0
WireConnection;129;1;317;0
WireConnection;128;0;83;0
WireConnection;128;1;315;0
WireConnection;122;0;117;0
WireConnection;122;1;117;0
WireConnection;131;0;128;0
WireConnection;131;1;129;0
WireConnection;131;2;130;0
WireConnection;131;3;314;0
WireConnection;121;0;112;0
WireConnection;121;1;120;0
WireConnection;149;0;324;0
WireConnection;149;4;327;0
WireConnection;149;2;160;0
WireConnection;149;3;161;0
WireConnection;319;0;131;0
WireConnection;123;0;122;0
WireConnection;123;1;124;0
WireConnection;334;0;149;0
WireConnection;125;0;121;0
WireConnection;125;1;123;0
WireConnection;144;0;325;0
WireConnection;144;4;328;0
WireConnection;144;3;178;0
WireConnection;135;0;125;0
WireConnection;135;1;320;0
WireConnection;264;0;265;0
WireConnection;264;1;255;0
WireConnection;257;0;264;0
WireConnection;257;1;256;0
WireConnection;171;0;144;0
WireConnection;171;1;335;0
WireConnection;134;0;135;0
WireConnection;134;1;136;0
WireConnection;330;0;257;0
WireConnection;175;0;171;0
WireConnection;177;0;176;0
WireConnection;177;1;134;0
WireConnection;145;0;177;0
WireConnection;145;1;175;0
WireConnection;258;0;145;0
WireConnection;258;1;331;0
WireConnection;48;0;47;0
WireConnection;48;1;45;0
WireConnection;332;0;258;0
WireConnection;364;0;48;0
WireConnection;52;1;358;0
WireConnection;52;0;359;0
WireConnection;37;2;280;0
WireConnection;37;3;30;0
WireConnection;37;5;322;0
WireConnection;362;0;54;0
WireConnection;343;0;228;0
WireConnection;216;0;222;0
WireConnection;216;1;251;0
WireConnection;216;2;224;0
WireConnection;251;0;186;0
WireConnection;223;0;254;0
WireConnection;223;1;214;0
WireConnection;38;2;279;0
WireConnection;38;3;36;0
WireConnection;38;5;322;0
WireConnection;54;0;55;0
WireConnection;54;1;52;0
WireConnection;54;2;53;0
WireConnection;282;0;16;1
WireConnection;173;0;340;0
WireConnection;173;1;336;0
WireConnection;31;0;307;0
WireConnection;39;2;278;0
WireConnection;39;3;41;0
WireConnection;39;5;322;0
WireConnection;41;0;290;0
WireConnection;41;1;300;0
WireConnection;41;2;31;2
WireConnection;174;0;173;0
WireConnection;338;0;174;0
WireConnection;222;0;223;0
WireConnection;293;0;17;1
WireConnection;229;0;216;0
WireConnection;283;0;16;3
WireConnection;356;0;50;2
WireConnection;357;0;49;2
WireConnection;360;0;49;1
WireConnection;273;0;10;1
WireConnection;285;0;16;2
WireConnection;274;0;10;2
WireConnection;254;0;346;0
WireConnection;295;0;17;3
WireConnection;294;0;17;2
WireConnection;228;0;347;0
WireConnection;228;1;229;0
WireConnection;345;0;28;0
WireConnection;28;0;37;0
WireConnection;28;1;38;0
WireConnection;28;2;39;0
WireConnection;36;0;289;0
WireConnection;36;1;299;0
WireConnection;36;2;31;1
WireConnection;30;0;288;0
WireConnection;30;1;298;0
WireConnection;30;2;31;0
WireConnection;275;0;10;3
WireConnection;348;0;46;0
WireConnection;1;0;366;0
WireConnection;1;1;349;0
WireConnection;1;2;333;0
WireConnection;1;3;361;0
WireConnection;1;4;339;0
WireConnection;1;5;363;0
WireConnection;1;11;344;0
ASEEND*/
//CHKSM=82A822CCAEAF9A37D28BA11F7D3C1F4BD219B209